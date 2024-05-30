using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.Common;
using WarrAttachmentManagementService.Application.Interfaces;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Application.RepairOrderTestFile;

namespace WarrAttachmentManagementService.API.Controllers
{
    [Route("v1/repair-order-test-file")]
    [Route("qa/v1/repair-order-test-file")]
    public class RepairOrderTestFileController : Controller
    {
        private readonly IRepairOrderTestFileService repairOrderTestFile;

        public RepairOrderTestFileController(IRepairOrderTestFileService repairOrderTestFile)
        {
            this.repairOrderTestFile = repairOrderTestFile;
        }

        [HttpPost("upload-test-file")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UploadTestFileAsync([FromForm] UploadTestFileInputDto uploadTestFileInputDto,CancellationToken cancellationToken)
        {
            await repairOrderTestFile.UploadTestFileAsync(uploadTestFileInputDto, cancellationToken);
            return NoContent();
        }
        [HttpGet("get-download-url-for-template-file")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetDownloadURLForTemplateFile(AppTypeEnum appType, OperationType opType)
        {
            return Ok(await repairOrderTestFile.GetDownloadURLForTemplateFile(appType, opType));
        }
    }
}
