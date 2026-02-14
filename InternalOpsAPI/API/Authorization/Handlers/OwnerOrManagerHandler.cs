namespace API.Authorization.Handlers
{
    using System.Security.Claims;

    using API.Authorization.Requirements;

    using Microsoft.AspNetCore.Authorization;

    public class OwnerOrManagerHandler : AuthorizationHandler<OwnerOrManagerRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOrManagerRequirement requirement, string resourceOwnerId)
        {
            var user = context.User;

            if (user.IsInRole("Manager") || user.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == resourceOwnerId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}