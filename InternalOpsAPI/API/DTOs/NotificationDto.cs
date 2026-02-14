namespace API.DTOs
{
    using API.Models.Enums;

    public class NotificationDto
    {
        public int Id { get; set; }

        public required string Message { get; set; }

        public NotificationType Type { get; set; }

        public int? RelatedRequestId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }
}
