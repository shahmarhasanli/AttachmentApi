using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

namespace WarrAttachmentManagementService.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    private ILineItemAttachmentRepository? _lineItemAttachmentRepository;

    private ILineItemAttachmentTypeRepository? _lineItemAttachmentTypeRepository;

    private ILineItemRepository? _lineItemRepository;

    private IOemAttachmentLimitationRepository? _oemAttachmentLimitationRepository;

    private IRepairOrderAttachmentRepository? _repairOrderAttachmentRepository;

    private IRepairOrderRepository? _repairOrderRepository;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public ILineItemRepository LineItems =>
        _lineItemRepository ??= new LineItemRepository(_dbContext);

    public IRepairOrderRepository RepairOrders =>
        _repairOrderRepository ??= new RepairOrderRepository(_dbContext);

    public ILineItemAttachmentRepository LineAttachments =>
        _lineItemAttachmentRepository ??= new LineItemAttachmentRepository(_dbContext);

    public ILineItemAttachmentTypeRepository LineItemAttachmentTypes =>
        _lineItemAttachmentTypeRepository ??= new LineItemAttachmentTypeRepository(_dbContext);

    public IRepairOrderAttachmentRepository RepairOrderAttachments =>
        _repairOrderAttachmentRepository ??= new RepairOrderAttachmentRepository(_dbContext);

    public IOemAttachmentLimitationRepository OemAttachmentLimitations =>
        _oemAttachmentLimitationRepository ??= new OemAttachmentLimitationRepository(_dbContext);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}