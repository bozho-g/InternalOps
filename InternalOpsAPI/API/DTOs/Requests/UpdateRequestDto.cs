namespace API.DTOs.Requests
{
    using API.Models.Enums;

    public class UpdateRequestDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public RequestType? RequestType { get; set; }
    }
}
