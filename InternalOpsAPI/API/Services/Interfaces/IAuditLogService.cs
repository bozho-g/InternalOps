namespace API.Services.Interfaces
{
    using API.DTOs.AuditLogs;
    using API.Models.Enums;

    public interface IAuditLogService
    {
        public Task LogAsync(
            int requestId,
            string userId,
            AuditAction action,
            string? summary = null,
            Status? oldStatus = null,
            Status? newStatus = null,
            string? oldValue = null,
            string? newValue = null);

        public Task<List<AuditLogDto>> GetAuditLogsByRequest(int requestId);

        public Task<List<AuditLogDto>> GetAllAuditLogs(
            int? requestId = null,
            string? userId = null,
            AuditAction? action = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);

        public Task<int> GetAuditLogsCount(
            int? requestId = null,
            string? userId = null,
            AuditAction? action = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
    }
}
