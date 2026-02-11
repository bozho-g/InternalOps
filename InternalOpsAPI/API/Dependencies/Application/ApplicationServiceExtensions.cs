namespace API.Dependencies.Application
{
    using API.Services;
    using API.Services.Interfaces;

    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRequestService, RequestService>();

            services.AddHostedService<RefreshTokenService>();

            return services;
        }
    }
}
