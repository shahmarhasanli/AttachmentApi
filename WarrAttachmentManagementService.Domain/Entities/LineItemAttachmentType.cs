namespace WarrAttachmentManagementService.Domain.Entities;

public class LineItemAttachmentType : AuditableEntity
{
    public string AttachmentTypeCode { get; set; } = null!;
    public string AttachmentTypeDescription { get; set; } = null!;
}
