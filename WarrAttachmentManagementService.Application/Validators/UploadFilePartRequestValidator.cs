namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using MultiPartUploads;

public class UploadFilePartRequestValidator
    : AbstractValidator<UploadFilePartRequest>
{
    public UploadFilePartRequestValidator()
    {
        RuleFor(x => x.UploadId)
            .NotEmpty();

        RuleFor(x => x.PartNumber)
            .InclusiveBetween(1, 1000);

        RuleFor(x => x.FileKey)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .Must(x => x.Length <= 10_000_000) // 10 MB
            .WithMessage(x => $"File part size must be less than 10 MB. Actual size is {x.Content.Length / 1_000_000.0} MB");

        RuleFor(x => x.Content)
            .Must(x => x.Length > 5_242_880) // 5 MB
            .When(x => x.PartNumber == 1)
            .WithMessage(x => $"The first part size must be larger than 5 MB. Actual size is {x.Content.Length / 1_048_576.0} MB");
    }
}