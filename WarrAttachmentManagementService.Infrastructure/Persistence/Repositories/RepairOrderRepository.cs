using Microsoft.EntityFrameworkCore;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

internal class RepairOrderRepository
    : RepositoryBase<RepairOrder>, IRepairOrderRepository
{
    public RepairOrderRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> RepairOrderExists(Guid repairOrderId, CancellationToken cancellationToken)
    {
        return _dbSet.AnyAsync(x => x.RepairOrderId == repairOrderId, cancellationToken);
    }
}
