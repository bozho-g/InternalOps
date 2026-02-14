namespace API.Services.Interfaces
{
    using API.DTOs.Comments;

    public interface ICommentService
    {
        public Task<CommentDto> AddCommentAsync(string userId, int requestId, CreateCommentDto commentDto);
        public Task<CommentDto> UpdateCommentAsync(string userId, int commentId, UpdateCommentDto commentDto);
        public Task DeleteCommentAsync(string userId, int commentId);
    }
}
