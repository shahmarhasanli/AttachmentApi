namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface IAppDbContextInitializer
{
    void Migrate();
}
