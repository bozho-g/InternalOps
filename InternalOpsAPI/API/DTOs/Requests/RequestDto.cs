namespace API.DTOs.Requests
{
    using API.DTOs;
    using API.Models.Enums;

    public class RequestDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public RequestType RequestType { get; set; }

        public Status Status { get; set; }

        public required UserDto RequestedBy { get; set; }

        public UserDto? HandledBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
