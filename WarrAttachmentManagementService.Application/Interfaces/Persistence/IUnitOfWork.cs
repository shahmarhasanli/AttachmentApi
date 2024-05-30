namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    ILineItemRepository LineItems { get; }

    ILineItemAttachmentRepository LineAttachments { get; }

    ILineItemAttachmentTypeRepository LineItemAttachmentTypes { get; }

    IRepairOrderRepository RepairOrders { get; }

    IRepairOrderAttachmentRepository RepairOrderAttachments { get; }

    IOemAttachmentLimitationRepository OemAttachmentLimitations { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
