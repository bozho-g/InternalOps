namespace API.DTOs.Requests
{
    using System.ComponentModel.DataAnnotations;

    using API.Models.Enums;

    public class CreateRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public RequestType RequestType { get; set; }
    }
}
