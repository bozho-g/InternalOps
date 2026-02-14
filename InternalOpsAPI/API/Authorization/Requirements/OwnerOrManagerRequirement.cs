namespace API.Authorization.Requirements
{
    using Microsoft.AspNetCore.Authorization;

    public class OwnerOrManagerRequirement : IAuthorizationRequirement
    {
    }
}
