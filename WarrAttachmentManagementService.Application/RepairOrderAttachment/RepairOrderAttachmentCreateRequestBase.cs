namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

public abstract class RepairOrderAttachmentCreateRequestBase
{
    public Guid? OemFamilyId { get; set; }
    public Guid RepairOrderId { get; set; }
    public string RepairOrderNumber { get; set; }
    public string AttachmentTypeCode { get; set; }
    public string Notes { get; set; }
}