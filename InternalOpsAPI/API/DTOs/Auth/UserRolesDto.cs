namespace API.DTOs.Auth
{
    public class UserRolesDto
    {
        public required string Id { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
