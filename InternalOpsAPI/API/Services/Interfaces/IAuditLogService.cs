namespace API.Services.Interfaces
{
    using API.DTOs.AuditLogs;
    using API.DTOs.Paging;
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

        public Task<PagedResponse<AuditLogDto>> GetAllAuditLogs(AuditFilterDto filter);
    }
}
