namespace API.Services
{
    using API.Data;
    using API.DTOs.AuditLogs;
    using API.Mappers;
    using API.Models;
    using API.Models.Enums;
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

        public async Task<List<AuditLogDto>> GetAllAuditLogs(int? requestId = null, string? userId = null, AuditAction? action = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = context.AuditLogs.AsNoTracking();

            if (requestId.HasValue)
            {
                query = query.Where(a => a.RequestId == requestId);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(a => a.ChangedById == userId);
            }

            if (action.HasValue)
            {
                query = query.Where(a => a.Action == action);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.Timestamp >= fromDate);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.Timestamp <= toDate);
            }

            return await mapper.ProjectToDto(query.OrderByDescending(a => a.Timestamp)).ToListAsync();
        }

        public async Task<List<AuditLogDto>> GetAuditLogsByRequest(int requestId)
        {
            return await mapper.ProjectToDto(context.AuditLogs
                    .AsNoTracking()
                    .Where(log => log.RequestId == requestId))
                    .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        }

        public async Task<int> GetAuditLogsCount(int? requestId = null, string? userId = null, AuditAction? action = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = context.AuditLogs.AsNoTracking();

            if (requestId.HasValue)
            {
                query = query.Where(a => a.RequestId == requestId);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(a => a.ChangedById == userId);
            }

            if (action.HasValue)
            {
                query = query.Where(a => a.Action == action);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.Timestamp >= fromDate);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.Timestamp <= toDate);
            }

            return await query.CountAsync();
        }
    }
}
