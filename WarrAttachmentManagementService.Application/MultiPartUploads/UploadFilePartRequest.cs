namespace WarrAttachmentManagementService.Application.MultiPartUploads;

public class UploadFilePartRequest
{
    public string UploadId { get; init; } = null!;
    public int PartNumber { get; init; }
    public string FileKey { get; init; } = null!;
    public byte[] Content { get; init; } = null!;
}