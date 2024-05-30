using Microsoft.EntityFrameworkCore;
using WarrAttachmentManagementService.Application.Exceptions;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

internal class LineItemRepository
    : RepositoryBase<LineItem>, ILineItemRepository
{
    public LineItemRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<string> GetLineNumberFromLineItemAsync(
        Guid lineItemId,
        CancellationToken cancellationToken)
    {
        var lineNumber = await _dbSet
            .Where(x => x.LineItemId == lineItemId)
            .Select(x => x.LineNumber)
            .FirstOrDefaultAsync(cancellationToken);

        return lineNumber ?? throw new NotFoundException(lineItemId, nameof(LineItem));
    }

    public async Task<bool> LineItemExists(Guid lineItemId, CancellationToken cancellationToken)
    {
        var lineItem = await _dbSet.FirstOrDefaultAsync(x => x.LineItemId == lineItemId);

        if (lineItem != null)
        {
            return true;
        }

        return false;
    }
}
