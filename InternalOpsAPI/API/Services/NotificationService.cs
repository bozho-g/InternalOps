namespace API.Services
{
    using API.Data;
    using API.DTOs;
    using API.Hubs;
    using API.Models;
    using API.Models.Enums;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class NotificationService(AppDbContext dbContext, IHubContext<NotificationHub> hubContext) : INotificationService
    {
        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId) =>
            await dbContext.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    RelatedRequestId = n.RelatedRequestId,
                    Type = n.Type
                })
                .ToListAsync();

        public async Task MarkAllAsReadAsync(string userId) => await dbContext.Notifications.Where(n => n.UserId == userId).ExecuteUpdateAsync(n => n.SetProperty(n => n.IsRead, true));

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SendNotificationAsync(string userId, string message, int? relatedRequestId = null, NotificationType type = NotificationType.CommentAdded)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                RelatedRequestId = relatedRequestId,
                Type = type,
            };

            dbContext.Notifications.Add(notification);
            await dbContext.SaveChangesAsync();

            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                notification.Id,
                notification.Message,
                notification.CreatedAt,
                notification.RelatedRequestId,
                Type = notification.Type.ToString()
            });
        }
    }
}
