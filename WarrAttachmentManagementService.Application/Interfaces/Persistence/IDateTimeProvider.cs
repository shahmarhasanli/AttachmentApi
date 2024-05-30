namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface IDateTimeProvider
{
    DateTime GetNow();

    DateTime GetNowUtc();
}
