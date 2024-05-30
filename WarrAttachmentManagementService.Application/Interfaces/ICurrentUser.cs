namespace WarrAttachmentManagementService.Application.Interfaces;

public interface ICurrentUser
{
    Guid Id { get; }

    bool IsInRole(string role);
}
