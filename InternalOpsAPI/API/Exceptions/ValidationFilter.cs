using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Exceptions
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                throw new BadRequestException(string.Join(", ", errors));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
