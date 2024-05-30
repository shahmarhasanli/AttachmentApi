namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

using Microsoft.AspNetCore.Http;

public class RepairOrderAttachmentsCreateRequest
    : RepairOrderAttachmentCreateRequestBase
{
    public ICollection<IFormFile>? Files { get; set; }
}