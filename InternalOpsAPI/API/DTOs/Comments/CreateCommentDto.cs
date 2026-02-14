namespace API.DTOs.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCommentDto
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public required string Content { get; set; }
    }
}
