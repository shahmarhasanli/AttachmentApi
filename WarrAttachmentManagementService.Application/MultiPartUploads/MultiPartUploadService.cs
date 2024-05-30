namespace WarrAttachmentManagementService.Application.MultiPartUploads;

using System.Text.RegularExpressions;
using Common;
using Common.Constants;
using Domain.Entities;
using Exceptions;
using Interfaces;
using Interfaces.Persistence;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class MultiPartUploadService : IMultiPartUploadService
{
    private const string Bucket = "Buckets:Attachments";

    private readonly IConfiguration _configuration;
    private readonly ICurrentUser _currentUser;
    private readonly IFileService _fileService;

    private readonly IUnitOfWork _unitOfWork;

    public MultiPartUploadService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        ICurrentUser currentUser,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _currentUser = currentUser;
        _configuration = configuration;
    }

    public async Task<InitiateMultipartUploadResponse> InitiateMultipartUploadAsync(
        InitiateMultipartUploadRequest request,
        CancellationToken cancellationToken)
    {
        var repairOrderExists = await _unitOfWork
            .RepairOrders
            .RepairOrderExists(
                request.RepairOrderId,
                cancellationToken);

        if (!repairOrderExists)
            throw new NotFoundException(request.RepairOrderId, nameof(RepairOrder));

        string? lineNumber = null;

        if (request.LineItemId.HasValue)
        {
            lineNumber = await _unitOfWork
                .LineItems
                .GetLineNumberFromLineItemAsync(
                    request.LineItemId.Value, cancellationToken);

            if (lineNumber is null)
                throw new NotFoundException(request.LineItemId.Value, nameof(LineItem));
        }

        var filePath = request.LineItemId.HasValue
            ? request.LineItemId.ToString() + '_'
            : request.RepairOrderId.ToString() + '_';

        var existingFileNames = request.LineItemId.HasValue
            ? await GetExistingLineItemFileNamesAsync(
                request.LineItemId.Value,
                request.AttachmentTypeCode,
                cancellationToken)
            : await GetExistingRepairOrderFileNamesAsync(
                request.RepairOrderId,
                request.AttachmentTypeCode,
                cancellationToken);

        var nextAttachmentNumber = GetNextAttachmentNumber(
            existingFileNames,
            request.AttachmentTypeCode,
            request.FileContentType);

        var fileName = request.LineItemId.HasValue
            ? GenerateLineItemFileNameAsync(
                nextAttachmentNumber,
                lineNumber!,
                request.RepairOrderNumber,
                request.AttachmentTypeCode,
                request.FileContentType)
            : GenerateRepairOrderFileNameAsync(
                nextAttachmentNumber,
                request.RepairOrderNumber,
                request.AttachmentTypeCode,
                request.FileContentType);

        var fileKey = filePath + fileName;

        var uploadId = await _fileService.InitiateMultipartUploadAsync(
            _configuration[Bucket]!,
            fileKey,
            request.FileContentType,
            cancellationToken);

        return new InitiateMultipartUploadResponse(uploadId, fileKey);
    }

    public async Task<PartNumberETag> UploadFilePartAsync(
        UploadFilePartRequest request,
        CancellationToken cancellationToken)
    {
        var eTag = await _fileService.UploadPartAsync(
            _configuration[Bucket]!,
            request.UploadId,
            request.PartNumber,
            request.FileKey,
            request.Content,
            cancellationToken);

        return new PartNumberETag(request.PartNumber, eTag);
    }

    public async Task CompleteMultipartUploadAsync(
        CompleteMultipartUploadRequest request,
        CancellationToken cancellationToken)
    {
        await _fileService.CompleteMultipartUploadAsync(
            _configuration[Bucket]!,
            request.UploadId,
            request.FileKey,
            request.PartNumberETags,
            cancellationToken);

        var uploadedFileSize = await _fileService
            .GetFileSizeAsync(
                _configuration[Bucket]!,
                request.FileKey,
                cancellationToken);

        var oemLimitation = await _unitOfWork
            .OemAttachmentLimitations
            .GetByOemFamilyIdAsync(
                request.OemFamilyId,
                cancellationToken);

        if (oemLimitation is not null &&
            oemLimitation.MaxLength < uploadedFileSize)
        {
            await _fileService
                .DeleteFileAsync(
                    _configuration[Bucket]!,
                    request.FileKey,
                    cancellationToken);

            throw new ValidationException(new Dictionary<string, string[]>
            {
                {
                    "File.Length", new[]
                    {
                        $"Uploaded file size ({uploadedFileSize} bytes) " +
                        $"exceeds the maximum allowed file size ({oemLimitation.MaxLength} bytes)."
                    }
                }
            });
        }

        var approvedForClaimSubmission = _currentUser
            .IsInRole(UserRoles.WarrAgent);

        if (request.LineItemId.HasValue)
            AddLineItemAttachment(request, approvedForClaimSubmission);
        else
            AddRepairOrderAttachment(request, approvedForClaimSubmission);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private void AddRepairOrderAttachment(
        CompleteMultipartUploadRequest request,
        bool approvedForClaimSubmission)
    {
        var attachment = new RepairOrderAttachment
        {
            AttachmentTypeCode = request.AttachmentTypeCode,
            RepairOrderId = request.RepairOrderId,
            FileName = request.FileKey.Split('_')[1],
            OriginalName = request.OriginalFileName,
            Notes = request.Notes,
            ApprovedForClaimSubmission = approvedForClaimSubmission
        };

        _unitOfWork.RepairOrderAttachments
            .Add(attachment);
    }

    private void AddLineItemAttachment(
        CompleteMultipartUploadRequest request,
        bool approvedForClaimSubmission)
    {
        var attachment = new LineItemAttachment
        {
            AttachmentTypeCode = request.AttachmentTypeCode,
            LineItemId = request.LineItemId!.Value,
            FileName = request.FileKey.Split('_')[1],
            OriginalName = request.OriginalFileName,
            Notes = request.Notes,
            ApprovedForClaimSubmission = approvedForClaimSubmission
        };

        _unitOfWork.LineAttachments
            .Add(attachment);
    }

    private static string GenerateRepairOrderFileNameAsync(
        int attachmentNumber,
        string repairOrderNumber,
        string attachmentTypeCode,
        string fileContentType)
    {
        var extension = AttachmentUtilities
            .GetFileExtensionFromContentType(fileContentType);

        return $"{repairOrderNumber}." +
               $"{attachmentTypeCode}" +
               $"{attachmentNumber}" +
               $"{extension}".RemoveWhitespace();
    }

    private static string GenerateLineItemFileNameAsync(
        int attachmentNumber,
        string? lineNumber,
        string repairOrderNumber,
        string attachmentTypeCode,
        string fileContentType)
    {
        var extension = AttachmentUtilities
            .GetFileExtensionFromContentType(fileContentType);

        return $"{repairOrderNumber}." +
               $"{lineNumber}." +
               $"{attachmentTypeCode}" +
               $"{attachmentNumber}" +
               $"{extension}".RemoveWhitespace();
    }

    private static int GetNextAttachmentNumber(
        IEnumerable<string> existingFileNames,
        string attachmentTypeCode,
        string newFileContentType)
    {
        var extension = AttachmentUtilities
            .GetFileExtensionFromContentType(newFileContentType);

        var escapedAttachmentTypeCode = Regex.Escape(attachmentTypeCode);

        var fileNumberRegex = new Regex(@$"{escapedAttachmentTypeCode}(\d+)");

        return existingFileNames
                   .Where(fn => fn.EndsWith(extension))
                   .Select(fileName => fileNumberRegex.Match(fileName))
                   .Where(match => match.Success)
                   .Select(match => int.Parse(match.Groups[1].Value))
                   .DefaultIfEmpty()
                   .Max() +
               1;
    }

    private Task<List<string>> GetExistingRepairOrderFileNamesAsync(
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

    private Task<List<string>> GetExistingLineItemFileNamesAsync(
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
}