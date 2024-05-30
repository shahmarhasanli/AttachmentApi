using Microsoft.AspNetCore.Mvc;

namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

public class RepairOrderAttachmentApproveRequest
{
    [FromRoute(Name = "id")]
    public Guid RepairOrderId { get; set; }

    [FromBody]
    public ICollection<string> FileNames { get; set; }
}
