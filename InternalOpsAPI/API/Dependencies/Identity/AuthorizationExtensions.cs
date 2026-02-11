namespace API.Dependencies.Identity
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                 .AddPolicy("AdminAccess",
                     policy => policy.RequireRole("Admin"))
                 .AddPolicy("ManagerAccess",
                     policy => policy.RequireRole("Admin", "Manager"));

            return services;
        }
    }
}
