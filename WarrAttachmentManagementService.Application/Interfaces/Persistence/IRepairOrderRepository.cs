namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface IRepairOrderRepository
{
    Task<bool> RepairOrderExists(Guid repairOrderId, CancellationToken cancellationToken);
}
