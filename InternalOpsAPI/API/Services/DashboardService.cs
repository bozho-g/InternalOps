namespace API.Services
{
    using API.Data;
    using API.DTOs;
    using API.DTOs.Dashboards;
    using API.DTOs.Requests;
    using API.Models.Enums;
    using API.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class DashboardService(AppDbContext context) : IDashboardService
    {
        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var requests = context.Requests.AsNoTracking();
            var users = context.Users.AsNoTracking();

            var statsTask = requests
                .GroupBy(r => 1)
                .Select(g => new
                {
                    TotalRequests = g.Count(),
                    DeletedRequests = g.Count(r => r.IsDeleted)
                })
                .FirstOrDefaultAsync();

            var byStatusTask = requests
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status.ToString(), x => x.Count);

            var byTypeTask = requests
                .GroupBy(r => r.RequestType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type.ToString(), x => x.Count);

            var totalUsersTask = users.CountAsync();

            await Task.WhenAll(
                statsTask,
                byStatusTask,
                byTypeTask,
                totalUsersTask
            );

            var stats = statsTask.Result ?? new { TotalRequests = 0, DeletedRequests = 0 };

            return new()
            {
                ByStatus = byStatusTask.Result,
                ByType = byTypeTask.Result,
                TotalRequests = stats.TotalRequests,
                TotalUsers = totalUsersTask.Result,
                DeletedRequests = stats.DeletedRequests
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tmrw = today.AddDays(1);

            var requests = context.Requests.AsNoTracking();

            var byTypeTask = requests
                .GroupBy(r => r.RequestType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type.ToString(), x => x.Count);

            var pendingCountTask = requests
                .Where(r => r.Status == Status.Pending)
                .Select(r => new RequestDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    RequestType = r.RequestType,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    RequestedBy = new UserDto
                    {
                        Id = r.RequestedBy!.Id,
                        Email = r.RequestedBy.Email!
                    }
                })
                .ToListAsync();

            var countsTask = context.Requests
                .GroupBy(r => 1)
                .Select(g => new
                {
                    ApprovedToday = g.Count(r => r.Status == Status.Approved && r.UpdatedAt >= today && r.UpdatedAt < tmrw),
                    PendingRequests = g.Count(r => r.Status == Status.Pending)
                })
                .FirstOrDefaultAsync();

            await Task.WhenAll(countsTask, byTypeTask, pendingCountTask);

            var counts = countsTask.Result ?? new { ApprovedToday = 0, PendingRequests = 0 };

            return new()
            {
                ByType = byTypeTask.Result,
                PendingCount = counts.PendingRequests,
                ApprovedToday = counts.ApprovedToday,
                PendingRequests = pendingCountTask.Result,
            };
        }

        public async Task<UserDashboardDto> GetUserDashboardAsync(string userId)
        {
            var requests = context.Requests.AsNoTracking();

            var counts = await context.Requests
                 .AsNoTracking()
                 .Where(r => r.RequestedById == userId && !r.IsDeleted)
                 .GroupBy(r => 1)
                 .Select(g => new
                 {
                     MyApproved = g.Count(r => r.Status == Status.Approved),
                     MyPending = g.Count(r => r.Status == Status.Pending),
                     MyRejected = g.Count(r => r.Status == Status.Rejected)
                 })
                 .FirstOrDefaultAsync()
                 ?? new { MyApproved = 0, MyPending = 0, MyRejected = 0 };

            return new()
            {
                MyApproved = counts.MyApproved,
                MyPending = counts.MyPending,
                MyRejected = counts.MyRejected,
            };
        }
    }
}
