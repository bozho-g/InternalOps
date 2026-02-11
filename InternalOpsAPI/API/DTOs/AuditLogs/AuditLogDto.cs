namespace API.DTOs.AuditLogs
{
    using API.Models.Enums;

    public class AuditLogDto
    {
        public int Id { get; set; }
        public required UserDto ChangedBy { get; set; }
        public AuditAction Action { get; set; }
        public Status? OldStatus { get; set; }
        public Status? NewStatus { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Summary { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
