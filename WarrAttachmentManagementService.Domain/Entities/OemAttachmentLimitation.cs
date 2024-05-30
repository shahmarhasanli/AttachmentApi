namespace WarrAttachmentManagementService.Domain.Entities;

public class OemAttachmentLimitation
    : AuditableEntity
{
    public Guid OemFamilyId { get; set; }
    public int MaxLength { get; set; }
    public string? AllowedExtensions { get; set; }
}