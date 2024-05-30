using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

internal class LineItemAttachmentTypeRepository
    : RepositoryBase<LineItemAttachmentType>, ILineItemAttachmentTypeRepository
{
    public LineItemAttachmentTypeRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<ICollection<LineItemAttachmentType>> GetAllTypesAsync(CancellationToken cancellationToken)
    {
        return await QueryResultAsync(x => true, true).ToListAsync(cancellationToken);
    }

    public IQueryable<LineItemAttachmentType> QueryResultAsync(
        Expression<Func<LineItemAttachmentType, bool>> expression,
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
