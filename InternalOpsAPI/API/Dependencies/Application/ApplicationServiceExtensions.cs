namespace API.Dependencies.Application
{
    using API.Dependencies.Infrastructure;
    using API.Mappers;
    using API.Models;
    using API.Services;
    using API.Services.Interfaces;

    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMappers()
                .AddAzureStorage()
                .AddDomainServices(configuration)
                .AddBackgroundServices();

            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<UserMapper>();
            services.AddSingleton<CommentMapper>();
            services.AddSingleton<AttachmentMapper>();
            services.AddSingleton<RequestMapper>();
            services.AddSingleton<AuditLogMapper>();
            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IFileValidator, FileValidator>();
            services.AddScoped<INotificationService, NotificationService>();
            services.Configure<FileUploadOptions>(configuration.GetSection("FileUploadOptions"));
            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<CleanUpRefreshTokensService>();
            return services;
        }
    }
}
