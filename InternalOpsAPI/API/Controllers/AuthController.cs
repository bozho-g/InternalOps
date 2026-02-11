namespace API.Controllers
{
    using API.DTOs.Auth;
    using API.Services;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var result = await authService.Register(request);
            AppendRefreshCookie(result.RefreshToken!);
            return Ok(AuthResponse(result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            var result = await authService.Login(request);
            AppendRefreshCookie(result.RefreshToken!);
            return Ok(AuthResponse(result));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            await authService.Logout(User, refreshToken);

            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await authService.Refresh(refreshToken);

            if (!result.Success)
            {
                Response.Headers.Append("X-Auth-Error", "refresh_expired");
                return Unauthorized();
            }

            AppendRefreshCookie(result.RefreshToken!);
            return Ok(AuthResponse(result));
        }

        private void AppendRefreshCookie(string token)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        private static object AuthResponse(AuthResult r) => new
        {
            token = r.AccessToken,
            email = r.Email,
            roles = r.Roles
        };
    }
}
