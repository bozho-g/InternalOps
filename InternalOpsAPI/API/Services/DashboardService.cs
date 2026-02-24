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
            var requestStats = await context.Requests
                .AsNoTracking()
                .GroupBy(r => 1)
                .Select(g => new
                {
                    TotalRequests = g.Count(),
                    DeletedRequests = g.Count(r => r.IsDeleted),
                    ByStatus = g.GroupBy(r => r.Status)
                        .Select(sg => new { Status = sg.Key, Count = sg.Count() })
                        .ToList(),
                    ByType = g.GroupBy(r => r.RequestType)
                        .Select(tg => new { Type = tg.Key, Count = tg.Count() })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            var totalUsers = await context.Users.AsNoTracking().CountAsync();

            if (requestStats is null)
            {
                return new()
                {
                    ByStatus = [],
                    ByType = [],
                    TotalRequests = 0,
                    TotalUsers = totalUsers,
                    DeletedRequests = 0
                };
            }

            return new()
            {
                ByStatus = requestStats.ByStatus.ToDictionary(x => x.Status.ToString(), x => x.Count),
                ByType = requestStats.ByType.ToDictionary(x => x.Type.ToString(), x => x.Count),
                TotalRequests = requestStats.TotalRequests,
                TotalUsers = totalUsers,
                DeletedRequests = requestStats.DeletedRequests
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tmrw = today.AddDays(1);

            var stats = await context.Requests
                .AsNoTracking()
                .GroupBy(r => 1)
                .Select(g => new
                {
                    ByType = g.GroupBy(r => r.RequestType)
                        .Select(tg => new { Type = tg.Key, Count = tg.Count() })
                        .ToList(),
                    ApprovedToday = g.Count(r => r.Status == Status.Approved && r.UpdatedAt >= today && r.UpdatedAt < tmrw),
                    PendingCount = g.Count(r => r.Status == Status.Pending),
                    PendingRequests = g.Where(r => r.Status == Status.Pending)
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
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (stats is null)
            {
                return new()
                {
                    ByType = [],
                    ApprovedToday = 0,
                    PendingCount = 0,
                    PendingRequests = []
                };
            }

            return new()
            {
                ByType = stats.ByType.ToDictionary(x => x.Type.ToString(), x => x.Count),
                PendingCount = stats.PendingCount,
                ApprovedToday = stats.ApprovedToday,
                PendingRequests = stats.PendingRequests,
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
                     MyRejected = g.Count(r => r.Status == Status.Rejected),
                     MyCompleted = g.Count(r => r.Status == Status.Completed)
                 })
                 .FirstOrDefaultAsync()
                 ?? new { MyApproved = 0, MyPending = 0, MyRejected = 0, MyCompleted = 0 };

            return new()
            {
                MyApproved = counts.MyApproved,
                MyPending = counts.MyPending,
                MyRejected = counts.MyRejected,
                MyCompleted = counts.MyCompleted
            };
        }
    }
}
