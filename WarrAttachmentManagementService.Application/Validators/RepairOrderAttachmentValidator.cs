namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using RepairOrderAttachment;

public class RepairOrderAttachmentValidator
    : RepairOrderAttachmentValidatorBase<RepairOrderAttachmentCreateRequest>
{
    public RepairOrderAttachmentValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .SetValidator(new FormFileValidator());
    }
}