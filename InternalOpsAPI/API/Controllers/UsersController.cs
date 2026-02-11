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
        public IActionResult GetUsers()
        {
            return Ok(userService.GetUsers());
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var response = await userService.GetCurrentUserAsync(User);
            return Ok(response);
        }

        [Authorize(Policy = "AdminAccess")]
        [HttpGet("{userId}/requests")]
        public async Task<IActionResult> GetUserRequests(string userId)
        {
            var result = await requestService.GetAllRequests(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await requestService.GetAllRequests(userId);
            return Ok(result);
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