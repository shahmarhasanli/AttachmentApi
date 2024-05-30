using Microsoft.AspNetCore.Mvc;
using WarrAttachmentManagementService.Application.S3File;

namespace WarrAttachmentManagementService.Application.Interfaces.Services
{
    public interface IS3FileService
    {
        public Task<FileStreamResult> DownloadS3FileAsync(string bucketName,string Prefix,CancellationToken cancellationToken);
        public Task<List<S3FileDto>> GetS3FilesAsync(GetS3FilesRequestDto request, CancellationToken cancellationToken);
        public Task UploadFileAsync(S3FileUploadRequestDto requestDto,CancellationToken cancellationToken);

    }
}
