namespace WarrAttachmentManagementService.Application.MultiPartUploads;

public class InitiateMultipartUploadResponse
{
    public InitiateMultipartUploadResponse(
        string uploadId,
        string fileKey)
    {
        UploadId = uploadId;
        FileKey = fileKey;
    }

    public string UploadId { get; init; }
    public string FileKey { get; init; }
}