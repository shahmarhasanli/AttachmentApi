namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using LineItemAttachment;

public abstract class LineItemAttachmentValidatorBase<T>
    : AbstractValidator<T>
    where T : LineItemAttachmentCreateRequestBase
{
    protected LineItemAttachmentValidatorBase()
    {
        RuleFor(x => x.AttachmentTypeCode)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(x => x.RepairOrderNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LineItemId)
            .NotEmpty();
    }
}