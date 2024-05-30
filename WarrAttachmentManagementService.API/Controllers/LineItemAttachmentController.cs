using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WarrAttachmentManagementService.Application.Common.Constants;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Application.LineItemAttachment;
using WarrAttachmentManagementService.Application.Common.Models;

namespace WarrAttachmentManagementService.API.Controllers;

[Route("v1/line-item/attachment")]
[Route("qa/v1/line-item/attachment")]
public class LineItemAttachmentController : ApiControllerBase
{
    private readonly ILineItemAttachmentService _lineItemAttachmentService;

    public LineItemAttachmentController(
        ILineItemAttachmentService lineItemAttachmentService)
    {
        _lineItemAttachmentService = lineItemAttachmentService ?? throw new ArgumentNullException(nameof(lineItemAttachmentService));
    }

    [HttpGet("{lineItemId:guid}")]
    [ProducesResponseType(typeof(List<LineItemAttachmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAttachmentsListByLineItemId(
        Guid lineItemId,
        [FromQuery] bool onlyApproved,
        CancellationToken cancellationToken)
    {
        var response =
            await _lineItemAttachmentService.GetAttachmentsListByLineItemIdAsync(
                lineItemId,
                onlyApproved,
                cancellationToken);

        return Ok(response);
    }

    [HttpGet("{lineItemId:guid}/{fileName}")]
    [ProducesResponseType(typeof(FileStreamResult), 200)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadAttachmentAsync(
        Guid lineItemId,
        string fileName,
        CancellationToken cancellationToken)
    {
        return await _lineItemAttachmentService.DownloadLineAttachmentAsync(
            lineItemId,
            fileName,
            cancellationToken);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UploadAttachmentAsync(
        [FromForm] LineItemAttachmentCreateRequest lineItemAttachmentRequest,
        CancellationToken cancellationToken)
    {
        var createdLineItemAttachment = await _lineItemAttachmentService.UploadAttachmentAsync(
            lineItemAttachmentRequest,
            cancellationToken
            );

        return StatusCode((int)HttpStatusCode.Created, createdLineItemAttachment);
    }

    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UploadAttachmentsAsync(
        [FromForm] LineItemAttachmentsCreateRequest lineItemAttachmentsRequest,
        CancellationToken cancellationToken)
    {
        var createdLineItemAttachment = await _lineItemAttachmentService.UploadAttachmentsAsync(
            lineItemAttachmentsRequest,
            cancellationToken
        );

        return StatusCode((int)HttpStatusCode.Created, createdLineItemAttachment);
    }

    [HttpDelete("{lineItemId}/{fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAttachmentAsync(
        Guid lineItemId,
        string fileName,
        CancellationToken cancellationToken)
    {
        await _lineItemAttachmentService.DeleteAttachmentAsync(
            lineItemId,
            fileName,
            cancellationToken);

        return Ok();
    }

    [HttpDelete("{lineItemId}/by-file-names")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAttachmentsAsync(
        Guid lineItemId,
        IReadOnlyCollection<string> fileNames,
        CancellationToken cancellationToken)
    {
        await _lineItemAttachmentService.DeleteAttachmentsAsync(
            lineItemId,
            fileNames,
            cancellationToken);

        return Ok();
    }

    [HttpPost("approve/{id:guid}")]
    [Authorize(Roles = UserRoles.WarrUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ApproveAttachmentAsync(
        LineItemAttachmentApproveRequest approveRequest,
        CancellationToken cancellationToken)
    {
        await _lineItemAttachmentService.ApproveAttachmentAsync(
            approveRequest,
            cancellationToken);

        return Ok();
    }

    [HttpGet("url/{lineItemId:guid}/{fileName}")]
    [Authorize(Roles = UserRoles.WarrUser)]
    [ProducesResponseType(typeof(AttachmentUrlDto), StatusCodes.Status200OK)]
    public ActionResult GetAttachmentUrl(Guid lineItemId, string fileName)
    {
        var attachmentUrl = _lineItemAttachmentService
            .GetAttachmentUrl(lineItemId, fileName);

        return Ok(attachmentUrl);
    }
    [HttpGet("generate-name/{lineItemId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> GenerateNameForLineItemAttachmentAsync(Guid lineItemId, string repairOrderNumber, string attachmentTypeCode, string contentType, CancellationToken cancellationToken)
    {
        return Ok(await _lineItemAttachmentService.GenerateFileNameAsync(lineItemId, repairOrderNumber, attachmentTypeCode, contentType, cancellationToken));
    }
}
