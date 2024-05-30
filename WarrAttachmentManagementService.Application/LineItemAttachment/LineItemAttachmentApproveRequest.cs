using Microsoft.AspNetCore.Mvc;

namespace WarrAttachmentManagementService.Application.LineItemAttachment;

public class LineItemAttachmentApproveRequest
{
    [FromRoute(Name = "id")]
    public Guid LineItemId { get; set; }

    [FromBody]
    public ICollection<string> FileNames { get; set; }
}
