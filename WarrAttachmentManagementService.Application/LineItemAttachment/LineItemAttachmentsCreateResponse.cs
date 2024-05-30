namespace WarrAttachmentManagementService.Application.LineItemAttachment;

public class LineItemAttachmentsCreateResponse
{
    public LineItemAttachmentsCreateResponse(
        IReadOnlyCollection<string> fileNames,
        IReadOnlyCollection<string> originalNames)
    {
        FileNames = fileNames;
        OriginalNames = originalNames;
    }

    public IReadOnlyCollection<string> FileNames { get; init; }
    public IReadOnlyCollection<string> OriginalNames { get; init; }
}