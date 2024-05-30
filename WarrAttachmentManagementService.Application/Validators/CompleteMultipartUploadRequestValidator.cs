namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using MultiPartUploads;

public class CompleteMultipartUploadRequestValidator
    : AbstractValidator<CompleteMultipartUploadRequest>
{
    public CompleteMultipartUploadRequestValidator()
    {
        RuleFor(x => x.UploadId)
            .NotEmpty();

        RuleFor(x => x.FileKey)
            .NotEmpty();

        RuleFor(x => x.PartNumberETags)
            .NotEmpty()
            .ForEach(t =>
            {
                t.NotNull();
            });

        RuleFor(x => x.RepairOrderId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.LineItemId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.RepairOrderNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.AttachmentTypeCode)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(x => x.Notes)
            .NotEmpty()
            .MaximumLength(500);
    }
}