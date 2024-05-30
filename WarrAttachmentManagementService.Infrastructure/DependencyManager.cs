using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WARR.TicketManagement.Infrastructure.Persistence;
using WarrAttachmentManagementService.Application.Interfaces;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Infrastructure.Common;
using WarrAttachmentManagementService.Infrastructure.Persistence;
using WarrAttachmentManagementService.Infrastructure.Persistence.Interceptors;
using WarrAttachmentManagementService.Infrastructure.Services;

namespace WarrAttachmentManagementService.Infrastructure;

public static class DependencyManager
{
    public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddSingleton<IFileService, AmazonS3FileService>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddDatabaseRelatedServices(configuration);

        services.AddAwsServices();

        return services;
    }

    private static void AddDatabaseRelatedServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AppDbConnection"),
                builder => builder
                    .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("_AttachmentsDbMigrationHistory", "dbo"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAppDbContextInitializer, AppDbContextInitializer>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
    }

    private static void AddAwsServices(
        this IServiceCollection services)
    {
        services.AddAWSService<IAmazonS3>();
    }
}
