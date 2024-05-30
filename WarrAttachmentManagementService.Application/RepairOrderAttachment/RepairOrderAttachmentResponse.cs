namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

public class RepairOrderAttachmentResponse
{
    public Guid RepairOrderId { get; set; }
    public string? AttachmentTypeCode { get; set; }
    public string? FileName { get; set; }
    public string? OriginalName { get; set; }
    public string? Notes { get; set; }
    public bool ApprovedForClaimSubmission { get; set; }
    public DateTime CreatedDateTime { get; set; }
}
