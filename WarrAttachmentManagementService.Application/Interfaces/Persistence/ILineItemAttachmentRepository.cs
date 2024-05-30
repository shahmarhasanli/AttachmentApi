using System.Linq.Expressions;

namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface ILineItemAttachmentRepository
    : IRepositoryBase<Domain.Entities.LineItemAttachment>
{
    Task<ICollection<Domain.Entities.LineItemAttachment>> GetAttachmentsByLineItemIdAsync(
        Guid lineItemId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = false);

    Task<ICollection<Domain.Entities.LineItemAttachment>> GetApprovedAttachmentsByLineItemIdAsync(
        Guid lineItemId,
        CancellationToken cancellationToken,
        bool ignoreQueryFilters = false);

    IQueryable<Domain.Entities.LineItemAttachment> QueryResultAsync(
        Expression<Func<Domain.Entities.LineItemAttachment, bool>> expression,
        bool ignoreQueryFilters = false);
}
