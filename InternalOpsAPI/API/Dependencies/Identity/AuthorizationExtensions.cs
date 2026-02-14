namespace API.Dependencies.Identity
{
    using API.Authorization.Handlers;
    using API.Authorization.Requirements;

    using Microsoft.AspNetCore.Authorization;

    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, OwnerOrManagerHandler>();

            services.AddAuthorizationBuilder()
                 .AddPolicy("OwnerOrManager", policy => policy.AddRequirements(new OwnerOrManagerRequirement()))
                 .AddPolicy("AdminAccess",
                     policy => policy.RequireRole("Admin"))
                 .AddPolicy("ManagerAccess",
                     policy => policy.RequireRole("Admin", "Manager"));

            return services;
        }
    }
}
