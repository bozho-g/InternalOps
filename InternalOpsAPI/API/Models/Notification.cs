namespace API.Models
{
    using System.ComponentModel.DataAnnotations;

    using API.Models.Enums;

    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public required string Message { get; set; }

        [Required]
        public required string UserId { get; set; }

        public NotificationType Type { get; set; }

        public User? User { get; set; }

        public int? RelatedRequestId { get; set; }

        public Request? RelatedRequest { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
