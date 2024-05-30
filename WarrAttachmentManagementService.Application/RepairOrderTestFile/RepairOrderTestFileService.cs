using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrAttachmentManagementService.Application.Common;
using WarrAttachmentManagementService.Application.Exceptions;
using WarrAttachmentManagementService.Application.Interfaces;
using WarrAttachmentManagementService.Application.Interfaces.Services;

namespace WarrAttachmentManagementService.Application.RepairOrderTestFile
{
    public class RepairOrderTestFileService :IRepairOrderTestFileService
    {
        private readonly IFileService fileService;
        private readonly string _s3BucketName;

        public RepairOrderTestFileService(IFileService fileService)
        {
            this.fileService = fileService;
            this._s3BucketName = "warr-test-files";
        }
        public Task<string> GetDownloadURLForTemplateFile(AppTypeEnum appType, OperationType opType)
        {
            var key = $"drafts/{appType.ToString()}-{opType.ToString()}.xml";
            return Task.FromResult(fileService.GetFileUrl(_s3BucketName, key));
        }
        public async Task UploadTestFileAsync(UploadTestFileInputDto uploadTestFileInputDto,CancellationToken cancellation)
        {
            if (string.IsNullOrWhiteSpace(uploadTestFileInputDto.RepairOrderNumber))
                throw new ArgumentNullException(nameof(uploadTestFileInputDto.RepairOrderNumber));

            if (!uploadTestFileInputDto.RepairOrderNumber.EndsWith("test", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("repair order number must end with 'test'");

            var keySb = new StringBuilder();
            keySb.Append($"uploads/{uploadTestFileInputDto.AppType.ToString()}/{uploadTestFileInputDto.OperationType.ToString()}");
            keySb.Append($"/{uploadTestFileInputDto.RepairOrderNumber}.xml");
            await fileService.UploadAsync(_s3BucketName, keySb.ToString(), uploadTestFileInputDto.FormFile, cancellation);
        }
    }
}
