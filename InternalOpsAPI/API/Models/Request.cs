namespace API.Models
{
    using System.ComponentModel.DataAnnotations;

    using API.Models.Enums;

    public class Request
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public required string Title { get; set; }
        public string? Description { get; set; }

        public Status Status { get; set; }

        public RequestType RequestType { get; set; }

        [Required]
        public required string RequestedById { get; set; }
        public string? HandledById { get; set; }

        public required User RequestedBy { get; set; }
        public User? HandledBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedById { get; set; }
        public User? DeletedBy { get; set; }

        public List<RequestComment> Comments { get; set; } = [];
        public List<RequestAttachment> Attachments { get; set; } = [];
        public List<AuditLog> AuditLogs { get; set; } = [];

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
