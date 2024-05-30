using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.RepairOrderAttachment;
using WarrAttachmentManagementService.Application.Common.Models;

namespace WarrAttachmentManagementService.Application.Interfaces.Services;

public interface IRepairOrderAttachmentService
{
    Task<List<RepairOrderAttachmentResponse>> GetAttachmentsListByRepairOrderIdAsync(
        Guid repairOrderId,
        bool onlyApproved,
        CancellationToken cancellationToken);
    
    Task<FileStreamResult> DownloadRepairOrderAttachmentAsync(
        Guid repairOrderId,
        string fileName,
        CancellationToken cancellationToken);

    Task<RepairOrderAttachmentCreateResponse> UploadAttachmentAsync(
        RepairOrderAttachmentCreateRequest attachmentRequest,
        CancellationToken cancellationToken);

    Task<RepairOrderAttachmentsCreateResponse> UploadAttachmentsAsync(
        RepairOrderAttachmentsCreateRequest request,
        CancellationToken cancellationToken);

    Task DeleteAttachmentAsync(
        Guid repairOrderId,
        string fileName,
        CancellationToken cancellationToken);

    Task DeleteAttachmentsAsync(
        Guid repairOrderId,
        IReadOnlyCollection<string> fileNames,
        CancellationToken cancellationToken);

    Task ApproveAttachmentAsync(
        RepairOrderAttachmentApproveRequest approveRequest,
        CancellationToken cancellationToken);

    AttachmentUrlDto GetAttachmentUrl(Guid repairOrderId, string fileName);
}
