namespace API.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using API.Data;
    using API.DTOs.Paging;
    using API.DTOs.Users;
    using API.Exceptions;
    using API.Models;
    using API.Services.Extensions;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService(UserManager<User> userManager, AppDbContext context) : IUserService
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

        public async Task<PagedResponse<UserDashDto>> GetUsers(UserFilterDto filter)
        {
            var query = context.Users
                .AsNoTracking()
                .Select(u => new
                {
                    User = u,
                    TotalRequests = context.Requests.Count(r => r.RequestedById == u.Id)
                });

            if (!string.IsNullOrWhiteSpace(filter.Role))
            {
                switch (filter.Role.ToLower())
                {
                    case "user":
                        {
                            query = query.Where(x => !context.UserRoles.Any(ur => ur.UserId == x.User.Id));
                            break;
                        }

                    default:
                        {
                            query = query.Where(x =>
                        context.UserRoles
                           .Where(ur => ur.UserId == x.User.Id)
                           .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                             .Contains(filter.Role));
                            break;
                        }
                }
            }

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(u =>
                (u.User.Email != null && u.User.Email.Contains(filter.Search)));

            query = filter.Desc
                  ? query.OrderByDescending(x => x.TotalRequests)
                  : query.OrderBy(x => x.TotalRequests);

            var projected = query.Select(x => new UserDashDto
            {
                Id = x.User.Id,
                Email = x.User.Email,
                TotalRequests = x.TotalRequests,
                Roles = context.UserRoles
                    .Where(ur => ur.UserId == x.User.Id)
                    .Join(context.Roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => r.Name)
                    .ToList()!
            });

            return await projected.ToPagedResponseAsync(filter.PageNumber, filter.PageSize);
        }
    }
}