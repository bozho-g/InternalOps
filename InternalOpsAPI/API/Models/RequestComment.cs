namespace API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RequestComment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        public required string Content { get; set; }

        public int RequestId { get; set; }
        public Request? Request { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
