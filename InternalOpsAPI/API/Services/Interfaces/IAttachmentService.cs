namespace API.Services.Interfaces
{
    using API.DTOs.Attachments;
    using API.Models;

    public interface IAttachmentService
    {
        Task<AttachmentDto> UploadAttachmentAsync(string userId, int requestId, IFormFile file);
        Task DeleteAttachmentAsync(string userId, int attachmentId);
        Task DeleteAttachmentsForRequestAsync(string userId, Request request);
    }
}
