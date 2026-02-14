namespace API.Services.Interfaces
{
    using API.DTOs.Attachments;

    public interface IAttachmentService
    {
        Task<AttachmentDto> UploadAttachmentAsync(string userId, int requestId, IFormFile file);
        Task DeleteAttachmentAsync(string userId, int attachmentId);
    }
}
