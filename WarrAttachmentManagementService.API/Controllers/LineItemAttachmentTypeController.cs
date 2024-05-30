using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Application.LineItemAttachmentType;

namespace WarrAttachmentManagementService.API.Controllers;

[Route("v1/line-item/attachment-type")]
[Route("qa/v1/line-item/attachment-type")]
public class LineItemAttachmentTypeController : ApiControllerBase
{
    private readonly ILineItemAttachmentTypeService _lineItemAttachmentTypeService;

    public LineItemAttachmentTypeController(
        ILineItemAttachmentTypeService lineItemAttachmentTypeService)
    {
        _lineItemAttachmentTypeService = lineItemAttachmentTypeService ?? throw new ArgumentNullException(nameof(lineItemAttachmentTypeService));
    }

    [HttpGet()]
    [ProducesResponseType(typeof(List<LineItemAttachmentTypeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAttachmentTypesAsync(
        CancellationToken cancellationToken)
    {
        var response =
            await _lineItemAttachmentTypeService.GetAttachmentTypesAsync(
                cancellationToken);

        return Ok(response);
    }
}
