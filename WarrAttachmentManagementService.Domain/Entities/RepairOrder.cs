namespace WarrAttachmentManagementService.Domain.Entities;

public partial class RepairOrder : EntityBase
{
    public RepairOrder()
    {
        RepairOrderAttachments = new HashSet<RepairOrderAttachment>();
    }
    
    public Guid RepairOrderId { get; set; }
    public Guid RoofTopOemid { get; set; }
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public string? RepairOrderNumber { get; set; }
    public DateTime? OpenDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public decimal? Mileage { get; set; }
    public decimal? MileageOut { get; set; }
    public decimal? CustomerTotal { get; set; }
    public decimal? InternalTotal { get; set; }
    public decimal? WarrantyTotal { get; set; }
    public string? ServiceAdvisorNumber { get; set; }
    public string? ServiceAdvisorName { get; set; }
    public string? Comments { get; set; }
    public string? Dmsrostatus { get; set; }
    public int RepairOrderStatusEnum { get; set; }
    public int DocStatusEnum { get; set; }
    public DateTime? DocReceivedDateTime { get; set; }
    public Guid? DocReceivedUserId { get; set; }
    public bool HardCopyImageReceived { get; set; }
    public bool PostClaimAuditCompleted { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public bool Deleted { get; set; }
    public virtual ICollection<RepairOrderAttachment> RepairOrderAttachments { get; set; }

}
