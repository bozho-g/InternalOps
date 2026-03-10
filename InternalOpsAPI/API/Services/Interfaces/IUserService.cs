namespace API.Services.Interfaces
{
    using System.Security.Claims;

    using API.DTOs.Paging;
    using API.DTOs.Users;

    public interface IUserService
    {
        public Task<UserRolesDto?> GetCurrentUserAsync(ClaimsPrincipal principal);

        public Task ToggleManager(string userId);

        public Task<PagedResponse<UserDashDto>> GetUsers(UserFilterDto filter);
    }
}
