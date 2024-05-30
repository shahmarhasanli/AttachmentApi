namespace WarrAttachmentManagementService.Application.MultiPartUploads;

public class PartNumberETag
{
    public PartNumberETag()
    {
    }

    public PartNumberETag(int partNumber, string eTag)
    {
        PartNumber = partNumber;
        ETag = eTag;
    }

    public int PartNumber { get; init; }
    public string ETag { get; init; } = null!;
}