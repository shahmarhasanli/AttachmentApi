using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

internal class LineItemAttachmentRepository
    : RepositoryBase<LineItemAttachment>, ILineItemAttachmentRepository
{
    public LineItemAttachmentRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<ICollection<LineItemAttachment>> GetApprovedAttachmentsByLineItemIdAsync(
        Guid lineItemId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = false)
    {
        if (ignoreQueryFilters)
        {
            return await _dbSet
                    .IgnoreQueryFilters()
                    .Where(x => x.LineItemId == lineItemId &&
                                x.ApprovedForClaimSubmission == true)
                    .ToListAsync(cancellationToken);
        }
        return await _dbSet
            .Where(x => x.LineItemId == lineItemId &&
                        x.ApprovedForClaimSubmission == true)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<LineItemAttachment>> GetAttachmentsByLineItemIdAsync(
        Guid lineItemId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = true)
    {
        if (ignoreQueryFilters)
        {
            return await _dbSet
                    .IgnoreQueryFilters()
                    .Where(x => x.LineItemId == lineItemId)
                    .ToListAsync(cancellationToken);
        }
        return await _dbSet
            .Where(x => x.LineItemId == lineItemId)
            .ToListAsync(cancellationToken);
    }

    public IQueryable<LineItemAttachment> QueryResultAsync(
        Expression<Func<LineItemAttachment, bool>> expression,
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
