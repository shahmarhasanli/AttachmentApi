namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

public class RepairOrderAttachmentsCreateResponse
{
    public RepairOrderAttachmentsCreateResponse(
        IReadOnlyCollection<string> fileNames,
        IReadOnlyCollection<string> originalNames)
    {
        FileNames = fileNames;
        OriginalNames = originalNames;
    }

    public IReadOnlyCollection<string> FileNames { get; init; }
    public IReadOnlyCollection<string> OriginalNames { get; init; }
}