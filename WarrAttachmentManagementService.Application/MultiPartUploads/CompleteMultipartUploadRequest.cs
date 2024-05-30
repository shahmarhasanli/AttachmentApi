namespace WarrAttachmentManagementService.Application.MultiPartUploads;

public class CompleteMultipartUploadRequest
{
    public Guid OemFamilyId { get; init; }
    public string UploadId { get; init; } = null!;
    public string FileKey { get; init; } = null!;
    public ICollection<PartNumberETag> PartNumberETags { get; init; } = null!;
    public Guid RepairOrderId { get; init; }
    public Guid? LineItemId { get; init; }
    public string RepairOrderNumber { get; init; } = null!;
    public string AttachmentTypeCode { get; init; } = null!;
    public string Notes { get; init; } = null!;
    public string OriginalFileName { get; init; } = null!;
}