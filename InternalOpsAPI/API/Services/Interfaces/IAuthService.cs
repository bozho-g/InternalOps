namespace API.Services.Interfaces
{
    using API.DTOs.Users;

    public interface IAuthService
    {
        Task<AuthResult> Register(RegisterUserDto request);
        Task<AuthResult> Login(LoginUserDto request);
        Task<AuthResult> Refresh(string refreshToken);
        Task RevokeToken(string? refreshToken);
    }
}
