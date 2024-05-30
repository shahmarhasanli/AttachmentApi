using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrAttachmentManagementService.Application.Common.Models
{
    public class FileInformationDto
    {
        public string FileName { get; set; }
        public string FileUrl { get;set; }
        public DateTime UploadDateTime { get; set; }
    }
}
