using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WarrAttachmentManagementService.Application.Common.Constants;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Application.RepairOrderAttachment;
using WarrAttachmentManagementService.Application.Common.Models;

namespace WarrAttachmentManagementService.API.Controllers;

[Route("v1/repair-order/attachment")]
[Route("qa/v1/repair-order/attachment")]
public class RepairOrderAttachmentController : ApiControllerBase
{
    private readonly IRepairOrderAttachmentService _repairOrderAttachmentService;

    public RepairOrderAttachmentController(
        IRepairOrderAttachmentService repairOrderAttachmentService)
    {
        _repairOrderAttachmentService = repairOrderAttachmentService ?? throw new ArgumentNullException(nameof(repairOrderAttachmentService));
    }

    [HttpGet("{repairOrderId:guid}")]
    [ProducesResponseType(typeof(List<RepairOrderAttachmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAttachmentsListByRepairOrderId(
        Guid repairOrderId,
        [FromQuery] bool onlyApproved,
        CancellationToken cancellationToken)
    {
        var response =
            await _repairOrderAttachmentService.GetAttachmentsListByRepairOrderIdAsync(
                repairOrderId,
                onlyApproved,
                cancellationToken);

        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UploadAttachmentAsync(
        [FromForm] RepairOrderAttachmentCreateRequest repairOrderAttachmentRequest,
        CancellationToken cancellationToken)
    {
        var createdRepairOrderAttachment = await _repairOrderAttachmentService.UploadAttachmentAsync(
            repairOrderAttachmentRequest,
            cancellationToken
            );

        return StatusCode((int)HttpStatusCode.Created, createdRepairOrderAttachment);
    }

    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UploadAttachmentsAsync(
        [FromForm] RepairOrderAttachmentsCreateRequest repairOrderAttachmentsRequest,
        CancellationToken cancellationToken)
    {
        var createdRepairOrderAttachments = await _repairOrderAttachmentService
            .UploadAttachmentsAsync(
                repairOrderAttachmentsRequest,
                cancellationToken
            );

        return StatusCode((int)HttpStatusCode.Created, createdRepairOrderAttachments);
    }

    [HttpGet("{repairOrderId:guid}/{fileName}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadAttachmentAsync(
        Guid repairOrderId,
        string fileName,
        CancellationToken cancellationToken)
    {
        return await _repairOrderAttachmentService.DownloadRepairOrderAttachmentAsync(
            repairOrderId,
            fileName,
            cancellationToken);
    }

    [HttpDelete("{repairOrderId:guid}/{fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAttachmentAsync(
        Guid repairOrderId,
        string fileName,
        CancellationToken cancellationToken)
    {
        await _repairOrderAttachmentService.DeleteAttachmentAsync(
            repairOrderId,
            fileName,
            cancellationToken);

        return Ok();
    }

    [HttpDelete("{repairOrderId:guid}/by-file-names")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAttachmentsAsync(
        Guid repairOrderId,
        IReadOnlyCollection<string> fileNames,
        CancellationToken cancellationToken)
    {
        await _repairOrderAttachmentService.DeleteAttachmentsAsync(
            repairOrderId,
            fileNames,
            cancellationToken);

        return Ok();
    }

    [HttpPost("approve/{id:guid}")]
    [Authorize(Roles = UserRoles.WarrUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ApproveAttachmentAsync(
        RepairOrderAttachmentApproveRequest approveRequest,
        CancellationToken cancellationToken)
    {
        await _repairOrderAttachmentService.ApproveAttachmentAsync(
            approveRequest,
            cancellationToken);

        return Ok();
    }

    [HttpGet("url/{repairOrderId:guid}/{fileName}")]
    [Authorize(Roles = UserRoles.WarrUser)]
    [ProducesResponseType(typeof(AttachmentUrlDto), StatusCodes.Status200OK)]
    public ActionResult GetAttachmentUrl(Guid repairOrderId, string fileName)
    {
        var attachmentUrl = _repairOrderAttachmentService
            .GetAttachmentUrl(repairOrderId, fileName);

        return Ok(attachmentUrl);
    }
}
