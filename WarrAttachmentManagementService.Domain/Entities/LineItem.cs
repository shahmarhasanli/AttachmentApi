namespace WarrAttachmentManagementService.Domain.Entities;

public class LineItem : EntityBase
{
    public LineItem()
    {
        LineItemAttachments = new HashSet<LineItemAttachment>();
    }

    public Guid LineItemId { get; set; }
    public Guid RepairOrderId { get; set; }
    public string LineNumber { get; set; } = null!;
    public string? WorkType { get; set; }
    public string? DmstypeCode { get; set; }
    public string? DmslineStatus { get; set; }
    public string? Complaint { get; set; }
    public string? Cause { get; set; }
    public string? Story { get; set; }
    public decimal? StoryMileage { get; set; }
    public DateTime? StoryDate { get; set; }
    public int StoryConfidenceLevel { get; set; }
    public string? OpCode { get; set; }
    public string? OpCodeDescription { get; set; }
    public string? SpecialInstructions { get; set; }
    public string? OemdataField01 { get; set; }
    public string? OemdataField02 { get; set; }
    public string? OemdataField03 { get; set; }
    public string? OemdataField04 { get; set; }
    public string? OemdataField05 { get; set; }
    public string? OemdataField06 { get; set; }
    public string? OemdataField07 { get; set; }
    public string? OemdataField08 { get; set; }
    public string? OemdataField09 { get; set; }
    public string? OemdataField10 { get; set; }
    public string? OemdataField11 { get; set; }
    public string? OemdataField12 { get; set; }
    public string? OemdataField13 { get; set; }
    public string? OemdataField14 { get; set; }
    public string? OemdataField15 { get; set; }
    public string? OemdataField16 { get; set; }
    public string? OemdataField17 { get; set; }
    public string? OemdataField18 { get; set; }
    public string? OemdataField19 { get; set; }
    public string? OemdataField20 { get; set; }
    public string? OemdataField21 { get; set; }
    public string? OemdataField22 { get; set; }
    public string? OemdataField23 { get; set; }
    public string? OemdataField24 { get; set; }
    public string? OemdataField25 { get; set; }
    public string? OemdataField26 { get; set; }
    public string? OemdataField27 { get; set; }
    public string? OemdataField28 { get; set; }
    public string? OemdataField29 { get; set; }
    public string? OemdataField30 { get; set; }
    public string? TechNumber { get; set; }
    public string? TechName { get; set; }
    public decimal? BillingTime { get; set; }
    public decimal? StraightTime { get; set; }
    public decimal? LaborRate { get; set; }
    public decimal? LaborCharges { get; set; }
    public bool AutoSalesTaxIncluded { get; set; }
    public string? AutoSalesTaxDmscode { get; set; }
    public decimal? AutoSalesTaxAmount { get; set; }
    public int TimeStampConfidenceLevel { get; set; }
    public bool LineUpdatedByWarr { get; set; }
    public string? UpdateIntegrationStatus { get; set; }
    public int LineItemStatusEnum { get; set; }
    public int ClaimTypeEnum { get; set; }
    public Guid? TemplateCatalogId { get; set; }
    public int? OpCodeUpdateCounter { get; set; }
    public bool? AutoCloseRomaster { get; set; }
    public string? TransferLineNumber { get; set; }
    public int UpdateTempValue { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public bool Deleted { get; set; }
    public bool? AgentVerified { get; set; }

    public virtual ICollection<LineItemAttachment> LineItemAttachments { get; set; }
}
