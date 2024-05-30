namespace WarrAttachmentManagementService.Domain.Entities;

public abstract class AuditableEntity : EntityBase
{
    public Guid CreatedByID { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public Guid? UpdatedByID { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletionDateTime { get; set; }
}
