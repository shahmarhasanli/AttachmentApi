namespace WarrAttachmentManagementService.Domain.Entities;

public class LineItemAttachment : AuditableEntity
{
    public Guid LineItemId { get; set; }
    public string AttachmentTypeCode { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string? OriginalName { get; set; }
    public string? Notes { get; set; }
    public bool ApprovedForClaimSubmission { get; set; }
    public virtual LineItem LineItem { get; set; } = null!;
}
