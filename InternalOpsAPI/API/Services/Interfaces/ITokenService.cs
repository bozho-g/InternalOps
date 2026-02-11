namespace API.Services.Interfaces
{
    using API.DTOs.Auth;
    using API.Models;

    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
        RefreshTokensDto GenerateRefreshToken(string userId);
        string HashToken(string token);
    }
}
