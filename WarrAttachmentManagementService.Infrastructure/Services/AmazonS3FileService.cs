namespace WarrAttachmentManagementService.Infrastructure.Services;

using System.Net;
using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Exceptions;
using Application.Interfaces;
using Application.MultiPartUploads;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using WarrAttachmentManagementService.Application.Common.Models;
using CompleteMultipartUploadRequest = Amazon.S3.Model.CompleteMultipartUploadRequest;
using GetObjectRequest = Amazon.S3.Model.GetObjectRequest;
using InitiateMultipartUploadRequest = Amazon.S3.Model.InitiateMultipartUploadRequest;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;
using UploadPartRequest = Amazon.S3.Model.UploadPartRequest;

internal class AmazonS3FileService : IFileService
{
    private readonly IAmazonS3 _amazonS3;

    public AmazonS3FileService(
        IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public async Task<FileStreamResult> DownloadFileAsync(
        string bucketName,
        string key,
        string? name,
        CancellationToken cancellationToken)
    {
        var bucketExists = await _amazonS3
            .DoesS3BucketExistAsync(bucketName);

        if (!bucketExists)
            throw new NotFoundException($"S3 Bucket {bucketName} does not exist");

        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        var response = await _amazonS3.GetObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new AmazonS3FailureException("Download");

        await using var responseStream = response.ResponseStream;
        var stream = new MemoryStream();

        await responseStream.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        return new FileStreamResult(stream, response.Headers["Content-Type"])
        {
            FileDownloadName = name ?? key
        };
    }

    public string GetFileUrl(string bucketName, string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides =
            {
                ContentDisposition = "inline"
            }
        };

        return _amazonS3.GetPreSignedURL(request);
    }

    public async Task UploadAsync(
        string bucketName,
        string key,
        IFormFile file,
        CancellationToken cancellationToken,
        IDictionary<string, string> metadata = null)
    {
        var bucketExists = await _amazonS3
            .DoesS3BucketExistAsync(bucketName);

        if (!bucketExists)
            throw new NotFoundException($"S3 Bucket {bucketName} does not exist");

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = file.OpenReadStream()
        };

        request.Metadata.Add("Content-Type", file.ContentType);

        foreach (var entry in metadata)
        {
            request.Metadata.Add(entry.Key, entry.Value);
        }

        var response = await _amazonS3.PutObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new AmazonS3FailureException("Upload");
    }

    public async Task DeleteFileAsync(string bucketName, string key, CancellationToken cancellationToken)
    {
        var bucketExists = await _amazonS3
            .DoesS3BucketExistAsync(bucketName);

        if (!bucketExists)
            throw new NotFoundException($"S3 Bucket {bucketName} does not exist");

        var response = await _amazonS3.DeleteObjectAsync(bucketName, key, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.NoContent)
            throw new AmazonS3FailureException("Delete");
    }

    public async Task<string> InitiateMultipartUploadAsync(
        string bucketName,
        string fileKey,
        string fileContentType,
        CancellationToken cancellationToken)
    {
        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = fileKey,
            ContentType = fileContentType
        };

        initiateRequest.Metadata.Add("Content-Type", fileContentType);

        var response = await _amazonS3
            .InitiateMultipartUploadAsync(
                initiateRequest,
                cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new AmazonS3FailureException("InitiateMultipartUploadAsync");

        return response.UploadId;
    }

    public async Task<string> UploadPartAsync(
        string bucketName,
        string uploadId,
        int partNumber,
        string fileKey,
        byte[] content,
        CancellationToken cancellationToken)
    {
        var uploadPartRequest = new UploadPartRequest
        {
            BucketName = bucketName,
            Key = fileKey,
            UploadId = uploadId,
            PartNumber = partNumber,
            InputStream = new MemoryStream(content),
            PartSize = content.Length,
            MD5Digest = CalculateMd5(content)
        };

        var response = await _amazonS3
            .UploadPartAsync(
                uploadPartRequest,
                cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new AmazonS3FailureException("UploadPartAsync");

        return response.ETag;
    }

    public async Task CompleteMultipartUploadAsync(
        string bucketName,
        string uploadId,
        string fileKey,
        IEnumerable<PartNumberETag> partETags,
        CancellationToken cancellationToken)
    {
        var completeRequest = new CompleteMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = fileKey,
            UploadId = uploadId,
            PartETags = partETags
                .Select(e => new PartETag(e.PartNumber, e.ETag))
                .ToList()
        };

        var response = await _amazonS3.CompleteMultipartUploadAsync(
            completeRequest,
            cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new AmazonS3FailureException("CompleteMultipartUploadAsync");
    }

    public async Task<long> GetFileSizeAsync(
        string bucketName,
        string fileKey,
        CancellationToken cancellationToken)
    {
        var metadataRequest = new GetObjectMetadataRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        var metadataResponse = await _amazonS3
            .GetObjectMetadataAsync(metadataRequest, cancellationToken);

        if (metadataResponse.HttpStatusCode != HttpStatusCode.OK)
            throw new AmazonS3FailureException("GetFileSizeAsync");

        return metadataResponse.ContentLength;
    }

    private static string CalculateMd5(byte[] content)
    {
        using var md5 = MD5.Create();

        var hash = md5.ComputeHash(content);

        return Convert.ToBase64String(hash);
    }
    public async Task<List<FileInformationDto>> ListFilesAsync(
      string bucketName,
      string prefix,
      DateTime? dateFrom,
      DateTime? dateTo,
      CancellationToken cancellationToken,
      IDictionary<string, string> metaDataFilters = null)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = prefix
        };

        var result = new List<FileInformationDto>();
        ListObjectsV2Response response;

        do
        {
            response = await _amazonS3.ListObjectsV2Async(request, cancellationToken);
            foreach (var file in response.S3Objects)
            {
                var uploadDateTime = file.LastModified;

                if ((dateFrom == null || uploadDateTime >= dateFrom) &&
                    (dateTo == null || uploadDateTime <= dateTo))
                {
                    bool metadataMatches = true;

                    if (metaDataFilters != null && metaDataFilters.Count > 0)
                    {
                        var metadataResponse = await _amazonS3.GetObjectMetadataAsync(new GetObjectMetadataRequest
                        {
                            BucketName = bucketName,
                            Key = file.Key
                        }, cancellationToken);

                        foreach (var filter in metaDataFilters)
                        {
                            var metadataKey = $"x-amz-meta-{filter.Key.ToLower()}";
                            if (!metadataResponse.Metadata.Keys.Contains(metadataKey) ||
                                metadataResponse.Metadata[metadataKey] != filter.Value)
                            {
                                metadataMatches = false;
                                break;
                            }
                        }
                    }

                    if (metadataMatches)
                    {
                        result.Add(new FileInformationDto
                        {
                            FileName = Path.GetFileName(file.Key),
                            FileUrl = GetFileUrl(bucketName, file.Key),
                            UploadDateTime = uploadDateTime
                        });
                    }
                }
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);

        return result;
    }

}