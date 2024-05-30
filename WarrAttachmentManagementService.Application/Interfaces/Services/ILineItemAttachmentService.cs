using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.LineItemAttachment;
using WarrAttachmentManagementService.Application.Common.Models;

namespace WarrAttachmentManagementService.Application.Interfaces.Services;

public interface ILineItemAttachmentService
{
    Task<List<LineItemAttachmentResponse>> GetAttachmentsListByLineItemIdAsync(
        Guid lineItemId,
        bool onlyApproved,
        CancellationToken cancellationToken);

    Task<FileStreamResult> DownloadLineAttachmentAsync(
        Guid lineItemId,
        string fileName,
        CancellationToken cancellationToken);

    Task<LineItemAttachmentCreateResponse> UploadAttachmentAsync(
        LineItemAttachmentCreateRequest attachmentRequest,
        CancellationToken cancellationToken);

    Task<LineItemAttachmentsCreateResponse> UploadAttachmentsAsync(
        LineItemAttachmentsCreateRequest request,
        CancellationToken cancellationToken);

    Task DeleteAttachmentAsync(
        Guid lineItemId,
        string fileName,
        CancellationToken cancellationToken);

    Task DeleteAttachmentsAsync(
        Guid lineItemId,
        IReadOnlyCollection<string> fileNames,
        CancellationToken cancellationToken);

    Task ApproveAttachmentAsync(
        LineItemAttachmentApproveRequest approveRequest,
        CancellationToken cancellationToken);

    AttachmentUrlDto GetAttachmentUrl(Guid lineItemId, string fileName);
    Task<string> GenerateFileNameAsync(Guid lineItemId, string repairOrderNumber, string attachmentTypeCode, string contentType, CancellationToken cancellationToken);
}
