namespace API.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using API.Data;
    using API.DTOs.Auth;
    using API.Exceptions;
    using API.Models;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public sealed record AuthResult
    {
        public bool Success { get; init; }
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public string? Email { get; init; }
        public IEnumerable<string>? Roles { get; init; }
        public IEnumerable<string>? Errors { get; init; }
    }

    public class AuthService(AppDbContext context, ITokenService tokenService, UserManager<User> userManager) : IAuthService
    {
        public async Task<AuthResult> Login(LoginUserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            return await IssueTokens(user);
        }

        public async Task<AuthResult> Register(RegisterUserDto request)
        {
            if (await userManager.FindByEmailAsync(request.Email) is not null)
            {
                throw new ConflictException("Email is already in use");
            }

            var user = new User { Email = request.Email, UserName = request.Email };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return await IssueTokens(user);
        }

        public async Task Logout(ClaimsPrincipal principal, string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            var user = await userManager.GetUserAsync(principal);

            if (user is null)
            {
                return;
            }

            var incomingHash = tokenService.HashToken(refreshToken);

            var token = await context.RefreshTokens.FirstOrDefaultAsync(rt =>
                           rt.TokenHash == incomingHash &&
                           rt.UserId == user.Id &&
                           !rt.IsRevoked
                       );

            if (token is null)
            {
                return;
            }

            token.IsRevoked = true;
            await context.SaveChangesAsync();
        }

        public async Task<AuthResult> Refresh(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Fail("refresh_expired");
            }

            var incomingHash = tokenService.HashToken(refreshToken);

            var token = await context.RefreshTokens
                            .Include(rt => rt.User)
                            .FirstOrDefaultAsync(rt => rt.TokenHash == incomingHash);

            if (token is null || token.IsRevoked || token.Expires < DateTime.UtcNow)
            {
                if (token?.IsRevoked == true)
                {
                    await RevokeAll(token.UserId);
                }

                return Fail("refresh_expired");
            }

            token.IsRevoked = true;

            var newRefreshToken = tokenService.GenerateRefreshToken(token.UserId);
            await context.RefreshTokens.AddAsync(newRefreshToken.RefreshToken);
            await context.SaveChangesAsync();

            var roles = await userManager.GetRolesAsync(token.User);

            return new AuthResult
            {
                Success = true,
                AccessToken = await tokenService.GenerateToken(token.User),
                RefreshToken = newRefreshToken.TokenRaw,
                Email = token.User.Email!,
                Roles = roles,
            };
        }

        private async Task<AuthResult> IssueTokens(User user)
        {
            var refreshToken = tokenService.GenerateRefreshToken(user.Id);
            await context.RefreshTokens.AddAsync(refreshToken.RefreshToken);
            await context.SaveChangesAsync();

            var roles = await userManager.GetRolesAsync(user);

            return new AuthResult
            {
                Success = true,
                AccessToken = await tokenService.GenerateToken(user),
                RefreshToken = refreshToken.TokenRaw,
                Email = user.Email,
                Roles = roles,
            };
        }

        private async Task RevokeAll(string userId)
        {
            await context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteUpdateAsync(rt =>
                    rt.SetProperty(t => t.IsRevoked, true)
                );
        }

        private static AuthResult Fail(params string[] errors) => new() { Success = false, Errors = errors };
    }
}
