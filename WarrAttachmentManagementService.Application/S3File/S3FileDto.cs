using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrAttachmentManagementService.Application.S3File
{
    public class S3FileDto
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTime UploadDateTime { get; set; }
    }
}
