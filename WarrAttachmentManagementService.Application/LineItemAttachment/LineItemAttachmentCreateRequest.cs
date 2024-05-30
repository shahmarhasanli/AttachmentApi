namespace WarrAttachmentManagementService.Application.LineItemAttachment;

using Microsoft.AspNetCore.Http;

public class LineItemAttachmentCreateRequest
    : LineItemAttachmentCreateRequestBase
{
    public IFormFile? File { get; set; }
}