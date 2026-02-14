namespace API.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using API.DTOs.Auth;
    using API.Exceptions;
    using API.Models;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;

    public class UserService(UserManager<User> userManager) : IUserService
    {
        public async Task<UserRolesDto?> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal) ?? throw new BadRequestException("User not found");

            var roles = await userManager.GetRolesAsync(user);

            return new UserRolesDto
            {
                Id = user.Id,
                Email = user.Email,
                Roles = [.. roles]
            };
        }

        public async Task ToggleManager(string userId)
        {
            var user = (await userManager.FindByIdAsync(userId)) ?? throw new NotFoundException("User not found");

            if (await userManager.IsInRoleAsync(user, "Manager"))
            {
                await userManager.RemoveFromRoleAsync(user, "Manager");
            }
            else
            {
                await userManager.AddToRoleAsync(user, "Manager");
            }
        }

        public async Task<IEnumerable<UserRolesDto>> GetUsers()
        {
            var users = userManager.Users.ToList();
            var userRolesList = new List<UserRolesDto>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRolesList.Add(new UserRolesDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = [.. roles]
                });
            }
            return userRolesList;
        }
    }
}