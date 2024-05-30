using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.Common.Models;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Application.S3File;

namespace WarrAttachmentManagementService.API.Controllers
{
    [Route("v1/s3file")]
    [Route("qa/v1/s3file")]
    public class S3FileController : ApiControllerBase
    {
        private readonly IS3FileService _s3FileService;
        public S3FileController(IS3FileService rooftopAttachmentService)
        { 
            _s3FileService = rooftopAttachmentService;
        }

        [HttpPost("upload-s3-file")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UploadFilesAsync(S3FileUploadRequestDto requestDto,CancellationToken cancellationToken)
        {
            await  _s3FileService.UploadFileAsync(requestDto, cancellationToken);
            return NoContent();
        }
        [HttpPost("download-s3-file")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadFileasync(string bucketName, string prefix, CancellationToken cancellationToken)
        {
            return await _s3FileService.DownloadS3FileAsync(bucketName, prefix, cancellationToken);
        }

        [HttpPost("get-s3-files")]
        [ProducesResponseType(typeof(List<S3FileDto>), 200)]
        public async Task<IActionResult> GetS3FilesAsync(GetS3FilesRequestDto request,CancellationToken cancellationToken)
        {
            return Ok(await _s3FileService.GetS3FilesAsync(request, cancellationToken));
        }
    }
}
