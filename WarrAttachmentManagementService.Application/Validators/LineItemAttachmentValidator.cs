namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using LineItemAttachment;

public class LineItemAttachmentValidator
    : LineItemAttachmentValidatorBase<LineItemAttachmentCreateRequest>
{
    public LineItemAttachmentValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .SetValidator(new FormFileValidator());
    }
}