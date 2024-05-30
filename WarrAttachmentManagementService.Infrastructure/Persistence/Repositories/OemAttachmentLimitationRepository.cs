namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

using Application.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

internal class OemAttachmentLimitationRepository
    : RepositoryBase<OemAttachmentLimitation>,
        IOemAttachmentLimitationRepository
{
    public OemAttachmentLimitationRepository(
        AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public Task<OemAttachmentLimitation?> GetByOemFamilyIdAsync(
        Guid oemFamilyId,
        CancellationToken cancellationToken)
    {
        return _dbSet
            .Where(l => l.OemFamilyId == oemFamilyId &&
                        !l.Deleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}