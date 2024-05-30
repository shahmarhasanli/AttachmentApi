using WarrAttachmentManagementService.API.Services;
using WarrAttachmentManagementService.Application.Interfaces;

namespace WarrAttachmentManagementService.API;

public static class DependencyManager
{
    public static IServiceCollection AddAPIServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
