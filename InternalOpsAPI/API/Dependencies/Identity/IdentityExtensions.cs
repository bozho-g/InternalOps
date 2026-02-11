namespace API.Dependencies.Identity
{
    using API.Data;
    using API.Models;

    using Microsoft.AspNetCore.Identity;

    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
           .AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();

            return services;
        }
    }
}
