using API.DTOs.Users;
using API.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [Authorize(Policy = "ManagerAccess")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterDto filter)
        {
            return Ok(await userService.GetUsers(filter));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            return Ok(await userService.GetCurrentUserAsync(User));
        }

        [Authorize(Policy = "AdminAccess")]
        [HttpPost("{userId}/toggle-manager")]
        public async Task<IActionResult> ToggleManager(string userId)
        {
            await userService.ToggleManager(userId);
            return Ok();
        }
    }
}