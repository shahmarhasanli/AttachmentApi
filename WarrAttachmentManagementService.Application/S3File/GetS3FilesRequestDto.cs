using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrAttachmentManagementService.Application.S3File
{
    public class GetS3FilesRequestDto
    {
        public string BucketName { get; set; }
        public string Prefix { get; set; }
        public  Dictionary<string,string>? MetaData { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
