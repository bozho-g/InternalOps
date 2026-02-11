namespace API.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    using API.DTOs.Auth;
    using API.Models;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;

    public class TokenService(IConfiguration configuration, UserManager<User> userManager) : ITokenService
    {
        public async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role!));
            }

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokensDto GenerateRefreshToken(string userId)
        {
            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                TokenHash = HashToken(rawToken),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = userId,
                IsRevoked = false
            };

            return new RefreshTokensDto
            {
                TokenRaw = rawToken,
                RefreshToken = refreshToken
            };
        }

        public string HashToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
