namespace WarrAttachmentManagementService.Application.Common;

public static class AttachmentUtilities
{
    public static readonly Dictionary<string, string> AllowedContentTypeExtensions = new()
    {
        { "image/jpeg", ".jpg" },
        { "image/jpg", ".jpg" },
        { "image/png", ".png" },
        { "application/pdf", ".pdf" },
        { "text/csv", ".csv" },
        { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx" }
    };

    public static string GetFileExtensionFromContentType(string contentType)
    {
        if (contentType is null)
            throw new ArgumentNullException(nameof(contentType));

        if (!AllowedContentTypeExtensions.TryGetValue(contentType, out var extension))
            throw new ArgumentException($"Content type {contentType} is not supported. Allowed types are: {string.Join(", ", AllowedContentTypeExtensions.Values)}.");

        return extension;
    }
}