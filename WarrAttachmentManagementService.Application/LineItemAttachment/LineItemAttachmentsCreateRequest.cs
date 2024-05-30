namespace WarrAttachmentManagementService.Application.LineItemAttachment;

using Microsoft.AspNetCore.Http;

public class LineItemAttachmentsCreateRequest
    : LineItemAttachmentCreateRequestBase
{
    public ICollection<IFormFile>? Files { get; set; }
}