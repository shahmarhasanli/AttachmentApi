using Microsoft.AspNetCore.Mvc.Filters;
using WarrAttachmentManagementService.Application.Exceptions;

namespace WarrAttachmentManagementService.API.FilterAttributes;

public class ValidationFilterAttribute : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors  = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                                     kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
            
            var errorResponse = new Dictionary<string, string[]>();

            foreach(var error in errors)
            {
                var subErrors = new List<string>();

                foreach(var subError in error.Value)
                {
                    subErrors.Add(subError);
                }

                errorResponse.Add(error.Key, subErrors.ToArray());
            }

            throw new ValidationException(errorResponse);
        }

        await next();
    }
}
