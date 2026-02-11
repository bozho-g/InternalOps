namespace API.Models
{
    using API.Models.Enums;

    public class AuditLog
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public Request? Request { get; set; }

        public required string ChangedById { get; set; }
        public User? ChangedBy { get; set; }

        public AuditAction Action { get; set; }

        public Status? OldStatus { get; set; }
        public Status? NewStatus { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public string? Summary { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
