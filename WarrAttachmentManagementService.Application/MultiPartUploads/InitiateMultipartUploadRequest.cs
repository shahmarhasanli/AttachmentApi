namespace WarrAttachmentManagementService.Application.MultiPartUploads;

public class InitiateMultipartUploadRequest
{
    public Guid RepairOrderId { get; init; }
    public Guid? LineItemId { get; init; }
    public string RepairOrderNumber { get; init; } = null!;
    public string AttachmentTypeCode { get; init; } = null!;
    public string FileContentType { get; init; } = null!;
}