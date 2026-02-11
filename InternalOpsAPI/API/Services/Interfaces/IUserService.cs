namespace API.Services.Interfaces
{
    using System.Security.Claims;

    using API.DTOs.Auth;

    public interface IUserService
    {
        public Task<UserRolesDto?> GetCurrentUserAsync(ClaimsPrincipal principal);

        public Task ToggleManager(string userId);

        public Task<IEnumerable<UserRolesDto>> GetUsers();
    }
}
