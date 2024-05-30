using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrAttachmentManagementService.Application.Common.Models;
using WarrAttachmentManagementService.Application.RepairOrderTestFile;

namespace WarrAttachmentManagementService.Application.S3File
{
    public class S3FileUploadRequestDto
    {
        public IFormFile File { get; set; }
        public string BucketName { get; set; }
        public string Prefix { get; set; }
        public IDictionary<string, string>? MetaData { get; set; }
    }
}
