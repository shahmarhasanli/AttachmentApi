using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.LineItemAttachmentType;

namespace WarrAttachmentManagementService.Application.Interfaces.Services;

public interface ILineItemAttachmentTypeService
{
    Task<List<LineItemAttachmentTypeResponse>> GetAttachmentTypesAsync(
        CancellationToken cancellationToken);
}
