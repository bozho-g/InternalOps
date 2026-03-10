namespace API.DTOs.Users
{
    public class UserDashDto : UserRolesDto
    {
        public bool IsManager => Roles?.Contains("Manager") ?? false;

        public int TotalRequests { get; set; }
    }
}
