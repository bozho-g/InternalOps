using API.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService userService) : ControllerBase
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
            return Ok(await userService.GetCurrentUserAsync(User));
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