namespace API.Controllers
{
    using System.Security.Claims;

    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/attachments")]
    public class AttachmentsController(IAttachmentService attachmentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadAttachment([FromForm] int requestId, [FromForm] IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await attachmentService.UploadAttachmentAsync(userId!, requestId, file);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await attachmentService.DeleteAttachmentAsync(userId!, id);
            return NoContent();
        }
    }
}
