using System.Security.Claims;

using API.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService userService, IRequestService requestService) : ControllerBase
    {
        [Authorize(Policy = "AdminAccess")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await userService.GetUsers());
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            return Ok(await userService.GetCurrentUserAsync(base.User));
        }

        [Authorize(Policy = "AdminAccess")]
        [HttpGet("{userId}/requests")]
        public async Task<IActionResult> GetUserRequests(string userId)
        {
            return Ok(await requestService.GetAllRequests(userId));
        }

        [Authorize]
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.GetAllRequests(userId));
        }

        [Authorize(Policy = "AdminAccess")]
        [HttpPost("{userId}/toggle-manager")]
        public async Task<IActionResult> ChangeUserRole(string userId)
        {
            await userService.ToggleManager(userId);
            return Ok();
        }
    }
}