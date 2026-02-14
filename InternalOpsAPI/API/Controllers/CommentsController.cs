namespace API.Controllers
{
    using System.Security.Claims;

    using API.DTOs.Comments;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/comments")]
    public class CommentsController(ICommentService commentService) : ControllerBase
    {
        [HttpPost("{requestId}")]
        public async Task<IActionResult> AddComment(int requestId, [FromBody] CreateCommentDto commentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdComment = await commentService.AddCommentAsync(userId!, requestId, commentDto);

            return Ok(createdComment);
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] UpdateCommentDto commentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedComment = await commentService.UpdateCommentAsync(userId!, commentId, commentDto);
            return Ok(updatedComment);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await commentService.DeleteCommentAsync(userId!, commentId);
            return NoContent();
        }
    }
}
