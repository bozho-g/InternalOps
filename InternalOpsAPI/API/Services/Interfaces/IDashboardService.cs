namespace API.Services.Interfaces
{
    using API.DTOs.Dashboards;

    public interface IDashboardService
    {
        Task<UserDashboardDto> GetUserDashboardAsync(string userId);
        Task<ManagerDashboardDto> GetManagerDashboardAsync();
        Task<AdminDashboardDto> GetAdminDashboardAsync();
    }
}
