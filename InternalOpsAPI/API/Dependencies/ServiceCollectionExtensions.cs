namespace API.Dependencies
{
    using API.Dependencies.Application;
    using API.Dependencies.Identity;
    using API.Dependencies.Infrastructure;
    using API.Dependencies.Persistence;
    using API.Exceptions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddCorsPolicy(config)
                .AddDatabase(config)
                .AddIdentityServices()
                .AddJwtAuthentication(config)
                .AddAuthorizationPolicies()
                .AddApplicationServices(config)
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails();

            return services;
        }
    }
}
