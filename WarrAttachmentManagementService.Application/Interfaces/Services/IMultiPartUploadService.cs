namespace WarrAttachmentManagementService.Application.Interfaces.Services;

using MultiPartUploads;

public interface IMultiPartUploadService
{
    Task<InitiateMultipartUploadResponse> InitiateMultipartUploadAsync(
        InitiateMultipartUploadRequest request,
        CancellationToken cancellationToken);

    Task<PartNumberETag> UploadFilePartAsync(
        UploadFilePartRequest request,
        CancellationToken cancellationToken);

    Task CompleteMultipartUploadAsync(
        CompleteMultipartUploadRequest request,
        CancellationToken cancellationToken);
}