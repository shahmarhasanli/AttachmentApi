using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrAttachmentManagementService.Application.Common;
using WarrAttachmentManagementService.Application.RepairOrderTestFile;

namespace WarrAttachmentManagementService.Application.Interfaces.Services
{
    public interface IRepairOrderTestFileService
    {
        Task<string> GetDownloadURLForTemplateFile(AppTypeEnum appType, OperationType opType);

        Task UploadTestFileAsync(UploadTestFileInputDto uploadTestFileInputDto,CancellationToken cancellation);
    }
}
