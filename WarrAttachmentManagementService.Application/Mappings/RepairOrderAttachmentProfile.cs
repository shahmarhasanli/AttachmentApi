using AutoMapper;
using WarrAttachmentManagementService.Application.RepairOrderAttachment;

namespace WarrAttachmentManagementService.Application.Mappings;

public class RepairOrderAttachmentProfile : Profile
{
    public RepairOrderAttachmentProfile()
    {
        CreateMap<Domain.Entities.RepairOrderAttachment, RepairOrderAttachmentResponse>();
        CreateMap<Domain.Entities.RepairOrderAttachment, RepairOrderAttachmentCreateResponse>();
    }
}
