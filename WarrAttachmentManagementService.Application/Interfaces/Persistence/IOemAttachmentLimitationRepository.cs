namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

using Domain.Entities;

public interface IOemAttachmentLimitationRepository
    : IRepositoryBase<OemAttachmentLimitation>
{
    Task<OemAttachmentLimitation?> GetByOemFamilyIdAsync(
        Guid oemFamilyId,
        CancellationToken cancellationToken);
}