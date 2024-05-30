namespace WarrAttachmentManagementService.Application.Common.Constants;

public static class UserRoles
{
    public const string WarrAdmin = "0";
    public const string WarrAgent = "1";
    public const string ChirpUser = "4";
    public const string WarrAdminOrChirpUser = "0,4";
    public const string WarrUser = "0,1";
    public const string WarrOrChirpUser = "0,1,4";
}
