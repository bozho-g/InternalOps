namespace API.DTOs.Requests
{
    public class UserRequestsDto
    {
        public required UserDto User { get; set; }
        public IEnumerable<RequestDto> Requests { get; set; } = [];
    }
}
