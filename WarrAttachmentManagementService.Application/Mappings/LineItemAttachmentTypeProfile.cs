using AutoMapper;
using WarrAttachmentManagementService.Application.LineItemAttachmentType;

namespace WarrAttachmentManagementService.Application.Mappings;

public class LineItemAttachmentTypeProfile : Profile
{
    public LineItemAttachmentTypeProfile()
    {
        CreateMap<Domain.Entities.LineItemAttachmentType, LineItemAttachmentTypeResponse>();
    }
}
