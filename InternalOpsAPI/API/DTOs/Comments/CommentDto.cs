namespace API.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public UserDto Author { get; set; } = null!;
    }
}
