using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WarrAttachmentManagementService.Application.Exceptions;

namespace WarrAttachmentManagementService.API.FilterAttributes;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;

    public ApiExceptionFilterAttribute(
        IWebHostEnvironment environment,
        ILogger<ApiExceptionFilterAttribute> logger)
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ForbiddenException), HandleForbiddenException}
            };

        _environment = environment;
        _logger = logger;
    }


    public override void OnException(ExceptionContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();

        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        var details = new ProblemDetails()
        {
            Title = "Internal Server Error"
        };

        var exceptionMessage = context.Exception.Message;

        while (context.Exception.InnerException is not null)
        {
            context.Exception = context.Exception.InnerException;
            exceptionMessage += $" -> {context.Exception.Message}";
        }

        if (!_environment.IsProduction())
            details.Detail = exceptionMessage;

        if (!_environment.IsDevelopment())
            _logger.LogError("Unhandled exception: Message: {Message}, " +
                "Details: {@Exception}", exceptionMessage, context.Exception);

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails()
        {
            Title = "Resource was not found",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenException(ExceptionContext context)
    {
        var exception = (ForbiddenException)context.Exception;

        var details = new ProblemDetails()
        {
            Title = "Access Denied",
            Detail = exception.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;
    }
}
