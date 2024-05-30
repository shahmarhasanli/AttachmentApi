using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrAttachmentManagementService.Application.Common;

namespace WarrAttachmentManagementService.Application.RepairOrderTestFile
{
    public class UploadTestFileInputDto
    {
        public string RepairOrderNumber { get; set; }
        public IFormFile FormFile { get; set; }
        public AppTypeEnum AppType { get; set; }
        public OperationType OperationType { get; set; }
    }
}
