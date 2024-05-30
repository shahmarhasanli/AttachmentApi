namespace WarrAttachmentManagementService.Application.LineItemAttachment;

public abstract class LineItemAttachmentCreateRequestBase
{
    public Guid? OemFamilyId { get; set; }
    public Guid LineItemId { get; set; }
    public string RepairOrderNumber { get; set; }
    public string AttachmentTypeCode { get; set; }
    public string? Notes { get; set; }
    public bool FileNameShouldBeSame { get; set; } = false;

}