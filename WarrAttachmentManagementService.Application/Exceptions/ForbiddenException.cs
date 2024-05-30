namespace WarrAttachmentManagementService.Application.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("User is not authorized to perform action")
    {
    }
}
