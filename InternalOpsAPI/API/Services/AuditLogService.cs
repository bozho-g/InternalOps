namespace API.Services
{
    using API.Data;
    using API.DTOs.AuditLogs;
    using API.DTOs.Paging;
    using API.Mappers;
    using API.Models;
    using API.Models.Enums;
    using API.Services.Extensions;
    using API.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class AuditLogService(AppDbContext context, AuditLogMapper mapper) : IAuditLogService
    {
        public async Task LogAsync(int requestId, string userId, AuditAction action, string? summary = null, Status? oldStatus = null, Status? newStatus = null, string? oldValue = null, string? newValue = null)
        {
            var logEntry = new AuditLog
            {
                RequestId = requestId,
                ChangedById = userId,
                Action = action,
                Summary = summary,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                OldValue = oldValue,
                NewValue = newValue,
                Timestamp = DateTime.UtcNow
            };

            context.AuditLogs.Add(logEntry);
            await context.SaveChangesAsync();
        }

        public async Task<PagedResponse<AuditLogDto>> GetAllAuditLogs(AuditFilterDto filter)
        {
            var query = context.AuditLogs.AsNoTracking();

            var from = filter.FromDate?.ToDateTime(TimeOnly.MinValue);
            var to = filter.ToDate?.ToDateTime(TimeOnly.MinValue).AddDays(1);

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(a =>
                (a.Summary != null && a.Summary.Contains(filter.Search)) ||
                (a.NewValue != null && a.NewValue.Contains(filter.Search)) ||
                (a.OldValue != null && a.OldValue.Contains(filter.Search)) ||
                (a.ChangedBy != null && a.ChangedBy.Email != null && a.ChangedBy.Email.Contains(filter.Search)));

            if (filter.RequestId.HasValue)
                query = query.Where(a => a.RequestId == filter.RequestId);

            if (!string.IsNullOrEmpty(filter.UserId))
                query = query.Where(a => a.ChangedById == filter.UserId);

            if (filter.Action.HasValue)
                query = query.Where(a => a.Action == filter.Action);

            if (filter.FromDate.HasValue)
                query = query.Where(a => a.Timestamp >= from);

            if (filter.ToDate.HasValue)
                query = query.Where(a => a.Timestamp <= to);

            query = query.OrderByDescending(a => a.Timestamp);

            var projected = mapper.ProjectToDto(query);

            return await projected.ToPagedResponseAsync(filter.PageNumber, filter.PageSize);
        }
    }
}
