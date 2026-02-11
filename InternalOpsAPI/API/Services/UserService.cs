namespace API.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using API.DTOs.Auth;
    using API.Exceptions;
    using API.Models;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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
            var user = userManager.FindByIdAsync(userId).Result ?? throw new BadRequestException("User not found");
            var isManager = userManager.IsInRoleAsync(user, "Manager").Result;

            if (isManager)
            {
                userManager.RemoveFromRoleAsync(user, "Manager").Wait();
            }
            else
            {
                userManager.AddToRoleAsync(user, "Manager").Wait();
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