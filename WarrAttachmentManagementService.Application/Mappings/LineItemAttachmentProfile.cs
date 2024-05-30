using AutoMapper;
using WarrAttachmentManagementService.Application.LineItemAttachment;

namespace WarrAttachmentManagementService.Application.Mappings;

public class LineItemAttachmentProfile : Profile
{
    public LineItemAttachmentProfile()
    {
        CreateMap<Domain.Entities.LineItemAttachment, LineItemAttachmentResponse>();
        CreateMap<Domain.Entities.LineItemAttachment, LineItemAttachmentCreateResponse>();
    }
}
