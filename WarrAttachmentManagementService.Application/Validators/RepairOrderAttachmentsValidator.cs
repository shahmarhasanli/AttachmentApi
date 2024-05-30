namespace WarrAttachmentManagementService.Application.Validators;

using FluentValidation;
using RepairOrderAttachment;

public class RepairOrderAttachmentsValidator
    : RepairOrderAttachmentValidatorBase<RepairOrderAttachmentsCreateRequest>
{
    public RepairOrderAttachmentsValidator()
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