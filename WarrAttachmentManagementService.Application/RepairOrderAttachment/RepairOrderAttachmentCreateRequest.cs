namespace WarrAttachmentManagementService.Application.RepairOrderAttachment;

using Microsoft.AspNetCore.Http;

public class RepairOrderAttachmentCreateRequest 
    : RepairOrderAttachmentCreateRequestBase
{
    public IFormFile? File { get; set; }
}