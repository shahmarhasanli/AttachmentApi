namespace WarrAttachmentManagementService.Application.Exceptions;

public class AmazonS3FailureException : Exception
{
    public AmazonS3FailureException(string operation)
        : base($"{operation} operation has failed on Amazon S3")
    {
    }
}
