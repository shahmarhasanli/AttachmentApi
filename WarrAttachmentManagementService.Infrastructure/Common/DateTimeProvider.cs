using WarrAttachmentManagementService.Application.Interfaces.Persistence;

namespace WarrAttachmentManagementService.Infrastructure.Common;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetNow() => DateTime.Now;

    public DateTime GetNowUtc() => DateTime.UtcNow;
}
