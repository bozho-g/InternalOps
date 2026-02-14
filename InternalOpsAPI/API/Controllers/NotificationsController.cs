namespace API.Controllers
{
    using System.Security.Claims;

    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize]
    [Route("api/notifications")]
    public class NotificationsController(INotificationService notification) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await notification.GetUserNotificationsAsync(userId!);
            return Ok(notifications);
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await notification.MarkAsReadAsync(id);
            return NoContent();
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await notification.MarkAllAsReadAsync(userId!);
            return NoContent();
        }
    }
}
