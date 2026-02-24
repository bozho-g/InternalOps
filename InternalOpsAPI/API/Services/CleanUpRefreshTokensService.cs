namespace API.Services
{
    using System.Threading;

    using API.Data;

    using Microsoft.EntityFrameworkCore;

    public class CleanUpRefreshTokensService(IServiceScopeFactory scopeFactory, ILogger<CleanUpRefreshTokensService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Refresh Token Cleanup Service started.");

            await RunCleanupAsync(stoppingToken);

            var timer = new PeriodicTimer(TimeSpan.FromMinutes(15));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunCleanupAsync(stoppingToken);
            }

            logger.LogInformation("Refresh Token Cleanup Stopped");
        }

        private async Task RunCleanupAsync(CancellationToken stoppingToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var now = DateTime.UtcNow;

            var deleted = await context.RefreshTokens
                .Where(rt => rt.Expires < now || rt.IsRevoked)
                .ExecuteDeleteAsync(cancellationToken: stoppingToken);

            logger.LogInformation("Deleted {DeletedCount} expired/revoked refresh tokens", deleted);
        }
    }
}
