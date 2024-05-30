using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

internal class RepairOrderAttachmentRepository
    : RepositoryBase<RepairOrderAttachment>, IRepairOrderAttachmentRepository
{
    public RepairOrderAttachmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<RepairOrderAttachment>> GetApprovedAttachmentsByRepairOrderIdAsync(
        Guid repairOrderId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = true)
    {
        if (ignoreQueryFilters)
        {
            return await _dbSet
                    .IgnoreQueryFilters()
                    .Where(x => x.RepairOrderId == repairOrderId &&
                                x.ApprovedForClaimSubmission == true)
                    .ToListAsync(cancellationToken);
        }

        return await _dbSet
            .Where(x => x.RepairOrderId == repairOrderId &&
                        x.ApprovedForClaimSubmission == true)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<RepairOrderAttachment>> GetAttachmentsByRepairOrderIdAsync(
        Guid repairOrderId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = true)
    {
        if (ignoreQueryFilters)
        {
            return await _dbSet
                    .IgnoreQueryFilters()
                    .Where(x => x.RepairOrderId == repairOrderId)
                    .ToListAsync(cancellationToken);
        }

        return await _dbSet
            .Where(x => x.RepairOrderId == repairOrderId)
            .ToListAsync(cancellationToken);
    }

    public IQueryable<RepairOrderAttachment> QueryResultAsync(
        Expression<Func<RepairOrderAttachment, bool>> expression,
        bool ignoreQueryFilters)
    {
        if (ignoreQueryFilters)
        {
            return _dbSet
                .IgnoreQueryFilters()
                .Where(expression);
        }

        return _dbSet.Where(expression);
    }
}
