namespace API.Dependencies.Persistence
{
    using API.Data;

    using Microsoft.EntityFrameworkCore;

    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                               options.UseSqlServer(config.GetConnectionString("DefaultConnection"), options =>
                               {
                                   options.EnableRetryOnFailure(
                                       maxRetryCount: 5,
                                       maxRetryDelay: TimeSpan.FromSeconds(10),
                                       errorNumbersToAdd: null);
                               }));

            return services;
        }
    }
}
