using FluentValidation;
using Microsoft.AspNetCore.Http;
using WarrAttachmentManagementService.Application.Common;

namespace WarrAttachmentManagementService.Application.Validators;

public class FormFileValidator : AbstractValidator<IFormFile>
{
    public FormFileValidator()
    {
        RuleFor(x => x.Length)
            .LessThanOrEqualTo(10000000) // 10 MB
            .WithMessage("File size is larger than 10 MB");

        RuleFor(x => x.ContentType)
            .Must(x =>
                x.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("image/jpg", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("image/png", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("application/pdf", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase) ||
                x.Equals("text/csv", StringComparison.OrdinalIgnoreCase))

            .WithMessage($"File type is not allowed. Allowed types are: {string.Join(", ", AttachmentUtilities.AllowedContentTypeExtensions.Values)}.");
    }
}
