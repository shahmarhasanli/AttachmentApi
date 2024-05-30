using AutoMapper;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Application.Interfaces.Services;

namespace WarrAttachmentManagementService.Application.LineItemAttachmentType;

public class LineItemAttachmentTypeService : ILineItemAttachmentTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LineItemAttachmentTypeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LineItemAttachmentTypeResponse>> GetAttachmentTypesAsync(
        CancellationToken cancellationToken)
    {
        var attachmentTypes = await _unitOfWork.LineItemAttachmentTypes.GetAllTypesAsync(
            cancellationToken);

        return _mapper.Map<List<LineItemAttachmentTypeResponse>>(attachmentTypes);
    }
}
