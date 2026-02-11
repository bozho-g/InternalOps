namespace API.Exceptions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            var (statusCode, title, detail) = exception switch
            {
                BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = exception.GetType().Name,
            }, cancellationToken);

            return true;
        }
    }
}
