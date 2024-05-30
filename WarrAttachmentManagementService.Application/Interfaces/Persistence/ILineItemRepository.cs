namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface ILineItemRepository
    : IRepositoryBase<Domain.Entities.LineItem>
{
    Task<string> GetLineNumberFromLineItemAsync(Guid lineItemId, CancellationToken cancellationToken);

    Task<bool> LineItemExists(Guid lineItemId, CancellationToken cancellationToken);
}
