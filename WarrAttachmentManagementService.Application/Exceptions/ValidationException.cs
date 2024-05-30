using FluentValidation.Results;

namespace WarrAttachmentManagementService.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException()
        : this("One or more validation failed")
    {
    }

    public ValidationException(IDictionary<string, string[]> failures)
        : this()
    {
        Errors = failures;
    }

    public IDictionary<string, string[]> Errors { get; }
}
