namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using RepairOrderAttachment;

public abstract class RepairOrderAttachmentValidatorBase<T>
    : AbstractValidator<T>
    where T : RepairOrderAttachmentCreateRequestBase
{
    protected RepairOrderAttachmentValidatorBase()
    {
        RuleFor(x => x.AttachmentTypeCode)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(x => x.RepairOrderNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Notes)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.RepairOrderId)
            .NotEmpty();
    }
}