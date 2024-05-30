namespace WarrAttachmentManagementService.Application.Validators;

using Common;
using FluentValidation;
using MultiPartUploads;

public class InitiateMultipartUploadRequestValidator
    : AbstractValidator<InitiateMultipartUploadRequest>
{
    public InitiateMultipartUploadRequestValidator()
    {
        RuleFor(x => x.AttachmentTypeCode)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(x => x.RepairOrderNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.RepairOrderId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.LineItemId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.FileContentType)
            .Must(c => AttachmentUtilities.AllowedContentTypeExtensions.ContainsKey(c))
            .WithMessage($"File type is not allowed. Allowed types are: {string.Join(", ", AttachmentUtilities.AllowedContentTypeExtensions.Values)}.");
    }
}