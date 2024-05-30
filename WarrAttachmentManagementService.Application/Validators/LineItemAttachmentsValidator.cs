namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using LineItemAttachment;

public class LineItemAttachmentsValidator
    : LineItemAttachmentValidatorBase<LineItemAttachmentsCreateRequest>
{
    public LineItemAttachmentsValidator()
    {
        RuleFor(x => x.Files)
            .NotNull()
            .ForEach(f =>
            {
                f.NotNull();
                f.SetValidator(new FormFileValidator());
            });
    }
}