namespace API.Controllers
{
    using System.Security.Claims;

    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/dashboards")]
    [ApiController]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        [HttpGet("user")]
        public async Task<IActionResult> GetUserDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dashboard = await dashboardService.GetUserDashboardAsync(userId!);

            return Ok(dashboard);
        }

        [Authorize(Policy = "ManagerAccess")]
        [HttpGet("manager")]
        public async Task<IActionResult> GetDashboard() => Ok(await dashboardService.GetManagerDashboardAsync());

        [Authorize(Policy = "AdminAccess")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDashboard() => Ok(await dashboardService.GetAdminDashboardAsync());
    }
}
