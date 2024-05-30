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

namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

internal class RepairOrderAttachmentService : IRepairOrderAttachmentService
{
    private const string Bucket = "Buckets:Attachments";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IConfiguration _configuration;

    public RepairOrderAttachmentService(
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

    public async Task<List<RepairOrderAttachmentResponse>> GetAttachmentsListByRepairOrderIdAsync(
        Guid repairOrderId,
        bool onlyApproved,
        CancellationToken cancellationToken)
    {
        var repairOrderAttachments = onlyApproved
            ? await _unitOfWork.RepairOrderAttachments.GetApprovedAttachmentsByRepairOrderIdAsync(
                repairOrderId,
                cancellationToken)
            : await _unitOfWork.RepairOrderAttachments.GetAttachmentsByRepairOrderIdAsync(
                repairOrderId,
                cancellationToken);

        return _mapper.Map<List<RepairOrderAttachmentResponse>>(repairOrderAttachments)!;
    }

    public async Task<RepairOrderAttachmentCreateResponse> UploadAttachmentAsync(
        RepairOrderAttachmentCreateRequest attachmentRequest,
        CancellationToken cancellationToken)
    {
        var repairOrderExists = await _unitOfWork
            .RepairOrders
            .RepairOrderExists(
                attachmentRequest.RepairOrderId,
                cancellationToken);

        if (!repairOrderExists)
            throw new NotFoundException(attachmentRequest.RepairOrderId, nameof(RepairOrder));

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

        var filepath = attachmentRequest.RepairOrderId.ToString() + '_';

        var existingFileNames = await GetExistingFileNamesAsync(
            attachmentRequest.RepairOrderId,
            attachmentRequest.AttachmentTypeCode,
            cancellationToken);

        var nextAttachmentNumber = GetNextAttachmentNumber(
            existingFileNames,
            attachmentRequest.AttachmentTypeCode,
            attachmentRequest.File!);

        var fileName = GenerateFileNameAsync(
            nextAttachmentNumber,
            attachmentRequest.RepairOrderNumber,
            attachmentRequest.AttachmentTypeCode,
            attachmentRequest.File!);

        var key = filepath + fileName;

        await _fileService.UploadAsync(
            _configuration[Bucket],
            key,
            attachmentRequest.File!,
            cancellationToken);

        var approvedForClaimSubmission = _currentUser
            .IsInRole(UserRoles.WarrAgent);

        var createdRepairOrderAttachment = new Domain.Entities.RepairOrderAttachment()
        {
            AttachmentTypeCode = attachmentRequest.AttachmentTypeCode,
            RepairOrderId = attachmentRequest.RepairOrderId,
            FileName = fileName,
            OriginalName = attachmentRequest.File!.FileName,
            Notes = attachmentRequest.Notes,
            ApprovedForClaimSubmission = approvedForClaimSubmission
        };

        _unitOfWork.RepairOrderAttachments
            .Add(createdRepairOrderAttachment);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return _mapper.Map<RepairOrderAttachmentCreateResponse>(createdRepairOrderAttachment);
    }

    public async Task<RepairOrderAttachmentsCreateResponse> UploadAttachmentsAsync(
        RepairOrderAttachmentsCreateRequest request,
        CancellationToken cancellationToken)
    {
        var repairOrderExists = await _unitOfWork
            .RepairOrders
            .RepairOrderExists(
                request.RepairOrderId,
                cancellationToken);

        if (!repairOrderExists)
            throw new NotFoundException(request.RepairOrderId, nameof(RepairOrder));

        var filepath = request.RepairOrderId.ToString() + '_';

        var existingFileNames = await GetExistingFileNamesAsync(
            request.RepairOrderId,
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

            var fileName = GenerateFileNameAsync(
                nextAttachmentNumber,
                request.RepairOrderNumber,
                request.AttachmentTypeCode,
                file);

            var key = filepath + fileName;

            await _fileService.UploadAsync(
                _configuration[Bucket],
                key,
                file,
                cancellationToken);

            var approvedForClaimSubmission = _currentUser
                .IsInRole(UserRoles.WarrAgent);

            var createdRepairOrderAttachment = new Domain.Entities.RepairOrderAttachment()
            {
                AttachmentTypeCode = request.AttachmentTypeCode,
                RepairOrderId = request.RepairOrderId,
                FileName = fileName,
                OriginalName = file.FileName,
                Notes = request.Notes,
                ApprovedForClaimSubmission = approvedForClaimSubmission
            };

            _unitOfWork.RepairOrderAttachments
                .Add(createdRepairOrderAttachment);

            newFileNames.Add(fileName);
            originalNames.Add(file.FileName);
            existingFileNames.Add(fileName);
        }

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return new RepairOrderAttachmentsCreateResponse(newFileNames, originalNames);
    }

    public async Task<FileStreamResult> DownloadRepairOrderAttachmentAsync(
        Guid repairOrderId,
        string fileName,
        CancellationToken cancellationToken)
    {
        var repairOrderAttachment = _unitOfWork.RepairOrderAttachments.QueryResultAsync(
                                            x => x.RepairOrderId == repairOrderId &&
                                            x.FileName == fileName)
                                            .FirstOrDefault();

        if (repairOrderAttachment == null)
        {
            throw new NotFoundException(repairOrderId.ToString(), fileName, nameof(RepairOrderAttachment));
        }

        string key = repairOrderId.ToString() + '_' + fileName;

        return await _fileService
            .DownloadFileAsync(_configuration[Bucket], key, repairOrderAttachment.OriginalName, cancellationToken);
    }

    public async Task DeleteAttachmentAsync(
        Guid repairOrderId,
        string fileName,
        CancellationToken cancellationToken)
    {
        var repairOrderAttachment = _unitOfWork.RepairOrderAttachments.QueryResultAsync(
                                                    x => x.RepairOrderId == repairOrderId &&
                                                    x.FileName == fileName)
                                                    .FirstOrDefault();

        if (repairOrderAttachment == null)
        {
            throw new NotFoundException(repairOrderId.ToString(), fileName, nameof(RepairOrderAttachment));
        }

        if (_currentUser.IsInRole(UserRoles.ChirpUser))
        {
            if (repairOrderAttachment.ApprovedForClaimSubmission)
            {
                throw new ForbiddenException();
            }
        }

        if (!repairOrderAttachment.ApprovedForClaimSubmission)
        {
            string key = repairOrderId.ToString() + '_' + fileName;

            await _fileService.DeleteFileAsync(_configuration[Bucket], key, cancellationToken);
        }

        _unitOfWork.RepairOrderAttachments.Delete(repairOrderAttachment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAttachmentsAsync(
        Guid repairOrderId,
        IReadOnlyCollection<string> fileNames,
        CancellationToken cancellationToken)
    {
        if (!fileNames.Any())
            throw new InvalidOperationException("File names must be provided");

        var repairOrderAttachments = await _unitOfWork.RepairOrderAttachments
            .QueryResultAsync(x =>
                x.RepairOrderId == repairOrderId &&
                fileNames.Contains(x.FileName))
            .ToListAsync(cancellationToken);

        if (!repairOrderAttachments.Any())
            return;

        if (_currentUser.IsInRole(UserRoles.ChirpUser))
            if (repairOrderAttachments.Any(a => a.ApprovedForClaimSubmission))
                throw new ForbiddenException();

        foreach (var attachment in repairOrderAttachments)
        {
            if (!attachment.ApprovedForClaimSubmission)
            {
                var key = repairOrderId.ToString() + '_' + attachment.FileName;

                await _fileService.DeleteFileAsync(_configuration[Bucket], key, cancellationToken);
            }

            _unitOfWork.RepairOrderAttachments.Delete(attachment);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateFileNameAsync(
        int attachmentNumber,
        string repairOrderNumber,
        string attachmentTypeCode,
        IFormFile file)
    {
        var extension = GetExtension(file);

        return $"{repairOrderNumber}." +
               $"{attachmentTypeCode}" +
               $"{attachmentNumber}" +
               $"{extension}".RemoveWhitespace();
    }

    public async Task ApproveAttachmentAsync(
        RepairOrderAttachmentApproveRequest approveRequest,
        CancellationToken cancellationToken)
    {
        foreach (var fileName in approveRequest.FileNames)
        {
            var repairOrderAttachment = _unitOfWork.RepairOrderAttachments.QueryResultAsync(
                                                       x => x.RepairOrderId == approveRequest.RepairOrderId &&
                                                       x.FileName == fileName)
                                                       .FirstOrDefault();

            if (repairOrderAttachment == null)
            {
                throw new NotFoundException(approveRequest.RepairOrderId.ToString(), fileName, nameof(RepairOrderAttachment));
            }

            if (repairOrderAttachment.ApprovedForClaimSubmission)
            {
                continue;
            }

            repairOrderAttachment.ApprovedForClaimSubmission = true;

            _unitOfWork.RepairOrderAttachments.Update(repairOrderAttachment);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public AttachmentUrlDto GetAttachmentUrl(Guid repairOrderId, string fileName)
    {
        var key = repairOrderId.ToString() + '_' + fileName;

        var url = _fileService.GetFileUrl(_configuration[Bucket], key);

        return new AttachmentUrlDto
        {
            Value = url
        };
    }

    private Task<List<string>> GetExistingFileNamesAsync(
        Guid repairOrderId,
        string attachmentTypeCode,
        CancellationToken cancellationToken)
    {
        return _unitOfWork
            .RepairOrderAttachments
            .QueryResultAsync(x =>
                    x.RepairOrderId == repairOrderId &&
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
