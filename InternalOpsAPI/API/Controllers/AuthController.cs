namespace API.Controllers
{
    using API.DTOs.Auth;
    using API.DTOs.Users;
    using API.Exceptions;
    using API.Models;
    using API.Services;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, UserManager<User> userManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var result = await authService.Register(request);
            AppendRefreshCookie(result.RefreshToken!);
            return Created("", AuthResponse(result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            var result = await authService.Login(request);
            AppendRefreshCookie(result.RefreshToken!);
            return Ok(AuthResponse(result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                await authService.RevokeToken(refreshToken);
                Response.Cookies.Delete("refreshToken");
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            var user = await userManager.GetUserAsync(User);

            var result = await userManager.ChangePasswordAsync(
                user!,
                request.CurrentPassword,
                request.NewPassword
            );

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return NoContent();
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await authService.Refresh(refreshToken!);

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
            role = r.Role?.ToLower()
        };
    }
}
