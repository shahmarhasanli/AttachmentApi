using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WarrAttachmentManagementService.Application.Common;
using WarrAttachmentManagementService.Application.Common.Constants;
using WarrAttachmentManagementService.Application.Exceptions;
using WarrAttachmentManagementService.Application.Interfaces;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WarrAttachmentManagementService.Application.Common.Models;


namespace WarrAttachmentManagementService.Application.LineItemAttachment;

internal class LineItemAttachmentService : ILineItemAttachmentService
{
    private const string Bucket = "Buckets:Attachments";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IConfiguration _configuration;

    public LineItemAttachmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService,
        ICurrentUser currentUser,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _currentUser = currentUser;
        _configuration = configuration;
    }

    public async Task<List<LineItemAttachmentResponse>> GetAttachmentsListByLineItemIdAsync(
        Guid lineItemId,
        bool onlyApproved,
        CancellationToken cancellationToken)
    {
        var lineAttachments = onlyApproved
            ? await _unitOfWork.LineAttachments.GetApprovedAttachmentsByLineItemIdAsync(
                lineItemId,
                cancellationToken)
            : await _unitOfWork.LineAttachments.GetAttachmentsByLineItemIdAsync(
                lineItemId,
                cancellationToken);

        return _mapper.Map<List<LineItemAttachmentResponse>>(lineAttachments);
    }

    public async Task<FileStreamResult> DownloadLineAttachmentAsync(
        Guid lineItemId,
        string fileName,
        CancellationToken cancellationToken)
    {
        var lineItemAttachment = _unitOfWork.LineAttachments.QueryResultAsync(
                                                   x => x.LineItemId == lineItemId &&
                                                   x.FileName == fileName)
                                                   .FirstOrDefault();

        if (lineItemAttachment == null)
        {
            throw new NotFoundException(lineItemId.ToString(), fileName, nameof(LineItemAttachment));
        }

        string key = lineItemId.ToString() + '_' + fileName;

        return await _fileService.DownloadFileAsync(_configuration[Bucket], key, lineItemAttachment.OriginalName, cancellationToken);
    }

    public async Task<LineItemAttachmentCreateResponse> UploadAttachmentAsync(
        LineItemAttachmentCreateRequest attachmentRequest,
        CancellationToken cancellationToken)
    {
        var lineNumber = await _unitOfWork
            .LineItems
            .GetLineNumberFromLineItemAsync(
                attachmentRequest.LineItemId, cancellationToken);

        if (lineNumber is null)
            throw new NotFoundException(attachmentRequest.LineItemId, nameof(LineItem));

        if (attachmentRequest.OemFamilyId.HasValue)
        {
            var oemLimitation = await _unitOfWork
                .OemAttachmentLimitations
                .GetByOemFamilyIdAsync(
                    attachmentRequest.OemFamilyId.Value,
                    cancellationToken);

            if (oemLimitation is not null &&
                oemLimitation.MaxLength < attachmentRequest.File!.Length)
            {
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    {
                        "File.Length", new[]
                        {
                            $"Uploaded file size ({attachmentRequest.File!.Length} bytes) " +
                            $"exceeds the maximum allowed file size ({oemLimitation.MaxLength} bytes)."
                        }
                    }
                });
            }
        }

        var filePath = attachmentRequest.LineItemId.ToString() + '_';

        var existingFileNames = await GetExistingFileNamesAsync(
            attachmentRequest.LineItemId,
            attachmentRequest.AttachmentTypeCode,
            cancellationToken);

        var nextAttachmentNumber = GetNextAttachmentNumber(
            existingFileNames,
            attachmentRequest.AttachmentTypeCode,
            attachmentRequest.File!);

        var fileName = attachmentRequest.FileNameShouldBeSame? attachmentRequest.File.FileName
            : GenerateFileNameAsync(
            nextAttachmentNumber,
            lineNumber,
            attachmentRequest.RepairOrderNumber,
            attachmentRequest.AttachmentTypeCode,
            attachmentRequest.File!);

        var key = filePath + fileName;

        await _fileService.UploadAsync(
            _configuration[Bucket],
            key,
            attachmentRequest.File!,
            cancellationToken);

        var approvedForClaimSubmission = _currentUser
            .IsInRole(UserRoles.WarrAgent);

        var createdLineItemAttachment = new Domain.Entities.LineItemAttachment()
        {
            AttachmentTypeCode = attachmentRequest.AttachmentTypeCode,
            LineItemId = attachmentRequest.LineItemId,
            FileName = fileName,
            OriginalName = attachmentRequest.File!.FileName,
            Notes = attachmentRequest.Notes,
            ApprovedForClaimSubmission = approvedForClaimSubmission,
        };

        _unitOfWork.LineAttachments
            .Add(createdLineItemAttachment);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return _mapper.Map<LineItemAttachmentCreateResponse>(createdLineItemAttachment);
    }

    public async Task<LineItemAttachmentsCreateResponse> UploadAttachmentsAsync(
        LineItemAttachmentsCreateRequest request,
        CancellationToken cancellationToken)
    {
        var lineNumber = await _unitOfWork
            .LineItems
            .GetLineNumberFromLineItemAsync(
                request.LineItemId, cancellationToken);

        if (lineNumber is null)
            throw new NotFoundException(request.LineItemId, nameof(LineItem));

        var filePath = request.LineItemId.ToString() + '_';

        var existingFileNames = await GetExistingFileNamesAsync(
            request.LineItemId,
            request.AttachmentTypeCode,
            cancellationToken);

        var newFileNames = new List<string>(request.Files!.Count);
        var originalNames = new List<string>(request.Files!.Count);

        foreach (var file in request.Files)
        {
            var nextAttachmentNumber = GetNextAttachmentNumber(
                existingFileNames,
                request.AttachmentTypeCode,
                file);

            var fileName = request.FileNameShouldBeSame? file.FileName:
                GenerateFileNameAsync(
                nextAttachmentNumber,
                lineNumber,
                request.RepairOrderNumber,
                request.AttachmentTypeCode,
                file);

            var key = filePath + fileName;

            await _fileService.UploadAsync(
                _configuration[Bucket],
                key,
                file,
                cancellationToken);

            var approvedForClaimSubmission = _currentUser
                .IsInRole(UserRoles.WarrAgent);

            var createdLineItemAttachment = new Domain.Entities.LineItemAttachment
            {
                AttachmentTypeCode = request.AttachmentTypeCode,
                LineItemId = request.LineItemId,
                FileName = fileName,
                OriginalName = file.FileName,
                Notes = request.Notes,
                ApprovedForClaimSubmission = approvedForClaimSubmission
            };

            _unitOfWork.LineAttachments
                .Add(createdLineItemAttachment);

            newFileNames.Add(fileName);
            originalNames.Add(file.FileName);
            existingFileNames.Add(fileName);
        }

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return new LineItemAttachmentsCreateResponse(newFileNames, originalNames);
    }
    public async Task DeleteAttachmentAsync(
        Guid lineItemId,
        string fileName,
        CancellationToken cancellationToken)
    {
        var lineItemAttachment = _unitOfWork.LineAttachments.QueryResultAsync(
                                                   x => x.LineItemId == lineItemId &&
                                                   x.FileName == fileName)
                                                   .FirstOrDefault();

        if (lineItemAttachment == null)
        {
            throw new NotFoundException(lineItemId.ToString(), fileName, nameof(LineItemAttachment));
        }

        if (_currentUser.IsInRole(UserRoles.ChirpUser))
        {
            if (lineItemAttachment.ApprovedForClaimSubmission)
            {
                throw new ForbiddenException();
            }
        }

        if (!lineItemAttachment.ApprovedForClaimSubmission)
        {
            string key = lineItemId.ToString() + '_' + fileName;

            await _fileService.DeleteFileAsync(_configuration[Bucket], key, cancellationToken);
        }

        _unitOfWork.LineAttachments.Delete(lineItemAttachment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAttachmentsAsync(
        Guid lineItemId,
        IReadOnlyCollection<string> fileNames,
        CancellationToken cancellationToken)
    {
        if (!fileNames.Any())
            throw new InvalidOperationException("File names must be provided");

        var lineItemAttachments = await _unitOfWork.LineAttachments
            .QueryResultAsync(x =>
                x.LineItemId == lineItemId &&
                fileNames.Contains(x.FileName))
            .ToListAsync(cancellationToken);

        if (!lineItemAttachments.Any())
            return;

        if (_currentUser.IsInRole(UserRoles.ChirpUser))
            if (lineItemAttachments.Any(a => a.ApprovedForClaimSubmission))
                throw new ForbiddenException();

        foreach (var attachment in lineItemAttachments)
        {
            if (!attachment.ApprovedForClaimSubmission)
            {
                var key = lineItemId.ToString() + '_' + attachment.FileName;

                await _fileService.DeleteFileAsync(_configuration[Bucket], key, cancellationToken);
            }

            _unitOfWork.LineAttachments.Delete(attachment);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateFileNameAsync(
        int attachmentNumber,
        string lineNumber,
        string repairOrderNumber,
        string attachmentTypeCode,
        IFormFile file)
    {
        var extension = GetExtension(file);

        return $"{repairOrderNumber}." +
               $"{lineNumber}." +
               $"{attachmentTypeCode}" +
               $"{attachmentNumber}" +
               $"{extension}".RemoveWhitespace();
    }

    public async Task ApproveAttachmentAsync(
        LineItemAttachmentApproveRequest approveRequest,
        CancellationToken cancellationToken)
    {
        foreach (var fileName in approveRequest.FileNames)
        {
            var lineItemAttachment = _unitOfWork.LineAttachments.QueryResultAsync(
                                                       x => x.LineItemId == approveRequest.LineItemId &&
                                                       x.FileName == fileName)
                                                       .FirstOrDefault();

            if (lineItemAttachment == null)
            {
                throw new NotFoundException(approveRequest.LineItemId.ToString(), fileName, nameof(LineItemAttachment));
            }

            if (lineItemAttachment.ApprovedForClaimSubmission)
            {
                continue;
            }

            lineItemAttachment.ApprovedForClaimSubmission = true;

            _unitOfWork.LineAttachments.Update(lineItemAttachment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public AttachmentUrlDto GetAttachmentUrl(Guid lineItemId, string fileName)
    {
        var key = lineItemId.ToString() + '_' + fileName;

        var url = _fileService
            .GetFileUrl(_configuration[Bucket], key);

        return new AttachmentUrlDto
        {
            Value = url
        };
    }
    public async Task<string> GenerateFileNameAsync(Guid lineItemId,string repairOrderNumber,string attachmentTypeCode, string contentType,CancellationToken cancellationToken)
    {


        var lineNumber = await _unitOfWork
            .LineItems
            .GetLineNumberFromLineItemAsync(
                lineItemId, cancellationToken);

        var extension = contentType;

        ExtensionsMapping.TryGetValue(extension, out extension);

        if(extension is null)
            throw new InvalidOperationException("Unable to determine file type");


        DateTime now = DateTime.UtcNow;
        var attachmentNumber = now.ToString("HHmmssfff");

        return $"{repairOrderNumber}." +
              $"{lineNumber}." +
              $"{attachmentTypeCode}" +
              $"{attachmentNumber}" +
              $"{extension}".RemoveWhitespace();
    }

    private Task<List<string>> GetExistingFileNamesAsync(
        Guid lineItemId,
        string attachmentTypeCode,
        CancellationToken cancellationToken)
    {
        return _unitOfWork
            .LineAttachments
            .QueryResultAsync(x =>
                    x.LineItemId == lineItemId &&
                    x.AttachmentTypeCode == attachmentTypeCode,
                true)
            .Select(x => x.FileName)
            .ToListAsync(cancellationToken);
    }

    private static int GetNextAttachmentNumber(
        IEnumerable<string> existingFileNames,
        string attachmentTypeCode,
        IFormFile file)
    {
        var extension = GetExtension(file);

        var escapedAttachmentTypeCode = Regex.Escape(attachmentTypeCode);

        var fileNumberRegex = new Regex(@$"{escapedAttachmentTypeCode}(\d+)");

        return existingFileNames
            .Where(fn => fn.EndsWith(extension))
            .Select(fileName => fileNumberRegex.Match(fileName))
            .Where(match => match.Success)
            .Select(match => int.Parse(match.Groups[1].Value))
            .DefaultIfEmpty()
            .Max() + 1;
    }

    private static string GetExtension(IFormFile file)
    {
        var type = file.ContentType;

        ExtensionsMapping.TryGetValue(type, out type);

        return type ?? throw new InvalidOperationException("Unable to determine file type");
    }

    private static readonly Dictionary<string, string> ExtensionsMapping = new()
    {
        { "image/jpeg", ".jpg" },
        { "image/jpg", ".jpg" },
        { "image/png", ".png" },
        { "application/pdf", ".pdf" },
        { "text/csv", ".csv" },
        { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx" }
    };


}
