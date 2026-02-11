namespace API.Services.Interfaces
{
    using System.Security.Claims;

    using API.DTOs.Auth;

    public interface IAuthService
    {
        Task<AuthResult> Register(RegisterUserDto request);
        Task<AuthResult> Login(LoginUserDto request);
        Task Logout(ClaimsPrincipal principal, string refreshToken);
        Task<AuthResult> Refresh(string refreshToken);
    }
}
