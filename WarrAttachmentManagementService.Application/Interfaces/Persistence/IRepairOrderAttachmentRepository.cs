using System.Linq.Expressions;

namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface IRepairOrderAttachmentRepository
    : IRepositoryBase<Domain.Entities.RepairOrderAttachment>
{
    Task<ICollection<Domain.Entities.RepairOrderAttachment>> GetAttachmentsByRepairOrderIdAsync(
        Guid repairOrderId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = false);

    Task<ICollection<Domain.Entities.RepairOrderAttachment>> GetApprovedAttachmentsByRepairOrderIdAsync(
        Guid repairOrderId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = false);

    IQueryable<Domain.Entities.RepairOrderAttachment> QueryResultAsync(
        Expression<Func<Domain.Entities.RepairOrderAttachment, bool>> expression,
        bool ignoreQueryFilter = false);
}
