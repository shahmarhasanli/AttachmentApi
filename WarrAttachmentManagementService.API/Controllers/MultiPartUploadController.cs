namespace WarrAttachmentManagementService.API.Controllers;

using System.Net;
using Application.Interfaces.Services;
using Application.MultiPartUploads;
using Microsoft.AspNetCore.Mvc;

[Route("v1/multipart-uploads")]
[Route("qa/v1/multipart-uploads")]
public class MultiPartUploadController : ApiControllerBase
{
    private readonly IMultiPartUploadService _multiPartUploadService;

    public MultiPartUploadController(
        IMultiPartUploadService multiPartUploadService)
    {
        _multiPartUploadService = multiPartUploadService ?? throw new ArgumentNullException(nameof(multiPartUploadService));
    }

    [HttpPost("initiate")]
    [ProducesResponseType(typeof(InitiateMultipartUploadResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> InitiateMultipartUpload(
        [FromBody] InitiateMultipartUploadRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _multiPartUploadService
            .InitiateMultipartUploadAsync(request, cancellationToken);

        return StatusCode((int)HttpStatusCode.Created, response);
    }

    [HttpPost("part")]
    [ProducesResponseType(typeof(PartNumberETag), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UploadFilePart(
        [FromBody] UploadFilePartRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _multiPartUploadService
            .UploadFilePartAsync(request, cancellationToken);

        return StatusCode((int)HttpStatusCode.Created, response);
    }

    [HttpPost("complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CompleteMultipartUpload(
        [FromBody] CompleteMultipartUploadRequest request,
        CancellationToken cancellationToken)
    {
        await _multiPartUploadService
            .CompleteMultipartUploadAsync(request, cancellationToken);

        return NoContent();
    }
}