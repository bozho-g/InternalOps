namespace API.Services.Interfaces
{
    using API.DTOs;
    using API.Models.Enums;

    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message, int? relatedRequestId = null, NotificationType type = NotificationType.CommentAdded);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);
    }
}
