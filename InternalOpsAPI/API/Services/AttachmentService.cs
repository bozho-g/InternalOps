namespace API.Services
{
    using API.Data;
    using API.DTOs.Attachments;
    using API.Exceptions;
    using API.Mappers;
    using API.Models;
    using API.Models.Enums;
    using API.Services.Extensions;
    using API.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class AttachmentService(AppDbContext context, IFileStorageService fileStorage, IAuditLogService auditLogService, AttachmentMapper mapper, IFileValidator fileValidator) : IAttachmentService
    {
        public async Task DeleteAttachmentAsync(string userId, int attachmentId)
        {
            var attachment = await context.RequestAttachments
                 .Include(a => a.Request)
                 .FirstOrDefaultAsync(a => a.Id == attachmentId);

            if (attachment == null)
                throw new NotFoundException("Attachment not found");

            if (attachment.Request?.RequestedById != userId)
                throw new UnauthorizedAccessException("You are not authorized to delete this attachment");

            if (attachment.Request.Status != Status.Pending)
                throw new BadRequestException("Cannot delete attachments from a request that is not pending");

            await fileStorage.DeleteFileAsync(attachment.FileUrl);
            context.RequestAttachments.Remove(attachment);

            await auditLogService.LogAttachmentRemovedAsync(attachment.RequestId, userId, attachment.FileName);
            await context.SaveChangesAsync();
        }

        public async Task<AttachmentDto> UploadAttachmentAsync(string userId, int requestId, IFormFile file)
        {
            var request = await context.Requests.FirstOrDefaultAsync(r => r.Id == requestId && !r.IsDeleted);

            if (request == null)
                throw new NotFoundException("Request not found");

            if (request.RequestedById != userId)
                throw new UnauthorizedAccessException("You are not authorized to upload attachments for this request");

            if (request.Status != Status.Pending)
                throw new BadRequestException("Cannot upload attachments to a request that is not pending");

            await fileValidator.ValidateAsync(file);

            var fileUrl = await fileStorage.UploadFileAsync(file, $"request-{requestId}");

            var attachment = new RequestAttachment
            {
                FileName = file.FileName,
                FileUrl = fileUrl,
                ContentType = file.ContentType,
                FileSize = file.Length,
                RequestId = requestId,
                UploadedAt = DateTime.UtcNow,
            };

            context.RequestAttachments.Add(attachment);
            await auditLogService.LogAttachmentAddedAsync(requestId, userId, file.FileName);

            await context.SaveChangesAsync();

            return mapper.MapToDto(attachment);
        }
    }
}
