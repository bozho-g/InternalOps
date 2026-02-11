namespace API.Dependencies.Infrastructure
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    var allowedOrigins = config
                             .GetSection("AllowedOrigins")
                             .Get<string[]>()
                             ?? throw new InvalidOperationException("AllowedOrigins is not configured.");

                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
            return services;
        }
    }
}
