using WarrAttachmentManagementService.Application.Interfaces;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WarrAttachmentManagementService.Application.S3File
{
    public class S3FileService : IS3FileService
    {
        private readonly IFileService _fileService;


        public S3FileService(
            IFileService fileService)
        {
            _fileService = fileService;
        }
        public async Task<FileStreamResult> DownloadS3FileAsync(string bucketName, string Prefix, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
            {
                throw new ArgumentNullException(nameof(bucketName));
            }

            if (string.IsNullOrWhiteSpace(Prefix))
            {
                throw new ArgumentNullException(nameof(Prefix));
            }

            return await _fileService.DownloadFileAsync(bucketName, Prefix, null, cancellationToken);
        }

        public async Task<List<S3FileDto>> GetS3FilesAsync(GetS3FilesRequestDto requestDto, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(requestDto.BucketName))
            {
                throw new ArgumentNullException(nameof(requestDto.BucketName));
            }

            if (string.IsNullOrWhiteSpace(requestDto.Prefix))
            {
                throw new ArgumentNullException(nameof(requestDto.Prefix));
            }

            var files = new List<S3FileDto>();

            var fileList = await _fileService.ListFilesAsync(requestDto.BucketName, requestDto.Prefix, requestDto.DateFrom,requestDto.DateTo, cancellationToken, requestDto.MetaData);

            foreach (var file in fileList)
            {
                files.Add(new S3FileDto
                {
                    FileName = file.FileName,
                    FileUrl = file.FileUrl,
                    UploadDateTime = file.UploadDateTime 
                                                       
                });
            }

            return files;
        }

        public async Task UploadFileAsync(S3FileUploadRequestDto requestDto,CancellationToken cancellationToken)
        {
            if (requestDto.File == null || requestDto.File?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(requestDto.File));
            }

            if(string.IsNullOrWhiteSpace(requestDto.BucketName))
            {
                throw new ArgumentNullException(nameof(requestDto.BucketName));
            }

            if(string.IsNullOrWhiteSpace(requestDto.Prefix))
            {
                throw new ArgumentNullException(nameof(requestDto.Prefix));
            }

            var s3Key = $"{requestDto.Prefix}";

           await _fileService.UploadAsync(requestDto.BucketName, s3Key, requestDto.File, cancellationToken, requestDto.MetaData);

        }


    }
}
