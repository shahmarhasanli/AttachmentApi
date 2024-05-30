using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Infrastructure.Persistence;

namespace WARR.TicketManagement.Infrastructure.Persistence;

internal class AppDbContextInitializer : IAppDbContextInitializer
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AppDbContextInitializer> _logger;

    public AppDbContextInitializer(
        ILogger<AppDbContextInitializer> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void Migrate()
    {
        try
        {
            if (_dbContext.Database.IsSqlServer())
                _dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}
