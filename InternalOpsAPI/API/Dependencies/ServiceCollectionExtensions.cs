namespace API.Dependencies
{
    using System.Text.Json.Serialization;

    using API.Dependencies.Application;
    using API.Dependencies.Identity;
    using API.Dependencies.Infrastructure;
    using API.Dependencies.Persistence;
    using API.Exceptions;

    using Microsoft.Extensions.Configuration;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddHttpContextAccessor()
                .AddControllers(options =>
                {
                    options.Filters.Add<ValidationFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails()
                .AddCorsPolicy(config)
                .AddDatabase(config)
                .AddIdentityServices()
                .AddJwtAuthentication(config)
                .AddAuthorizationPolicies()
                .AddApplicationServices(config)
                .AddSignalR();

            return services;
        }
    }
}
