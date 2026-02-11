namespace API.Services
{
    using System.Threading;

    using API.Data;

    using Microsoft.EntityFrameworkCore;

    public class RefreshTokenService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Refresh Token Cleanup Started");
            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var now = DateTime.UtcNow;

                var deleted = await context.RefreshTokens
                    .Where(rt => rt.Expires < now || rt.IsRevoked)
                    .ExecuteDeleteAsync(cancellationToken: stoppingToken);

                Console.WriteLine($"Deleted {deleted} expired/revoked refresh tokens");

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            Console.WriteLine("Refresh Token Cleanup Stopped");
        }
    }
}
