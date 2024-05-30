namespace WarrAttachmentManagementService.Application.Interfaces;

using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiPartUploads;
using WarrAttachmentManagementService.Application.Common.Models;

public interface IFileService
{
    Task<FileStreamResult> DownloadFileAsync(
        string bucketName,
        string key,
        string? name,
        CancellationToken cancellationToken);

    string GetFileUrl(string bucketName, string key);

    Task UploadAsync(
        string bucketName,
        string key,
        IFormFile file,
        CancellationToken cancellationToken,
        IDictionary<string,string> metadata = null);

    Task DeleteFileAsync(
        string bucketName,
        string key,
        CancellationToken cancellationToken);

    Task<string> InitiateMultipartUploadAsync(
        string bucketName,
        string fileKey,
        string fileContentType,
        CancellationToken cancellationToken);

    Task<string> UploadPartAsync(
        string bucketName,
        string uploadId,
        int partNumber,
        string fileKey,
        byte[] content,
        CancellationToken cancellationToken);

    Task CompleteMultipartUploadAsync(
        string bucketName,
        string uploadId,
        string fileKey,
        IEnumerable<PartNumberETag> partETags,
        CancellationToken cancellationToken);

    Task<long> GetFileSizeAsync(
        string bucketName, 
        string fileKey, 
        CancellationToken cancellationToken);

    Task<List<FileInformationDto>> ListFilesAsync(
        string bucketName,
        string prefix,
        DateTime? dateFrom,
        DateTime? dateTo,
        CancellationToken cancellationToken,
        IDictionary<string, string> metaDataFilters = null);


}