using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Application.LineItemAttachmentType;

public class LineItemAttachmentTypeResponse
{
    public string? AttachmentTypeCode { get; set; }
    public string? AttachmentTypeDescription { get; set; }
}
