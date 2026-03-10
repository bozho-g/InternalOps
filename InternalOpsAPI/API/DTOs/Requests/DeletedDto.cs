namespace API.DTOs.Requests
{
    public class DeletedDto
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedById { get; set; }
        public UserDto? DeletedBy { get; set; }
    }
}
