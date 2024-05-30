namespace WarrAttachmentManagementService.Domain.Entities;

public class RepairOrderAttachment : AuditableEntity
{
    public Guid RepairOrderId { get; set; }
    public string AttachmentTypeCode { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string? OriginalName { get; set; }
    public string Notes { get; set; } = null!;
    public bool ApprovedForClaimSubmission { get; set; }
    public virtual RepairOrder RepairOrder { get; set; } = null!;
}
