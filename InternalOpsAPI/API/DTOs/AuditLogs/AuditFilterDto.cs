namespace API.DTOs.AuditLogs
{
    using API.DTOs.Paging;
    using API.Models.Enums;

    public class AuditFilterDto : PagedFilter
    {
        public int? RequestId { get; set; }
        public string? UserId { get; set; }
        public AuditAction? Action { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
