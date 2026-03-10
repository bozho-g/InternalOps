namespace API.Services
{
    using API.Data;
    using API.DTOs.Comments;
    using API.Exceptions;
    using API.Mappers;
    using API.Models;
    using API.Models.Enums;
    using API.Services.Extensions;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;

    public class CommentService(AppDbContext context, CommentMapper mapper, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, INotificationService notificationService) : ICommentService
    {
        public async Task<CommentDto> AddCommentAsync(string userId, int requestId, CreateCommentDto commentDto)
        {
            var request = await context.Requests.FindAsync(requestId);

            if (request == null)
                throw new NotFoundException($"Request with ID {requestId} not found.");

            var user = httpContextAccessor.HttpContext?.User!;
            var authResult = await authorizationService.AuthorizeAsync(user, request.RequestedById, "OwnerOrManager");

            if (!authResult.Succeeded)
                throw new UnauthorizedException("You are not authorized to add comment to this request.");

            var comment = new RequestComment
            {
                Content = commentDto.Content,
                RequestId = requestId,
                UserId = userId,
            };

            context.RequestComments.Add(comment);
            await context.SaveChangesAsync();

            await auditLogService.LogCommentAddedAsync(requestId, userId, comment.Content);

            if (userId != request.RequestedById)
            {
                await notificationService.SendNotificationAsync(
                 request.RequestedById,
                 $"New comment on your request #{requestId}",
                 requestId,
                 NotificationType.CommentAdded
             );
            }
            else if (request.HandledById != null && userId != request.HandledById)
            {
                await notificationService.SendNotificationAsync(
                request.RequestedById,
                $"Requester replied on request #{requestId}",
                requestId,
                NotificationType.CommentAdded
                );
            }

            return mapper.MapToDto(comment);
        }

        public async Task DeleteCommentAsync(string userId, int commentId)
        {
            var comment = await context.RequestComments.FindAsync(commentId);

            if (comment == null)
                throw new NotFoundException($"Comment with ID {commentId} not found.");

            var user = httpContextAccessor.HttpContext?.User!;

            var authResult = await authorizationService.AuthorizeAsync(user, comment.UserId, "OwnerOrManager");

            if (!authResult.Succeeded)
                throw new UnauthorizedException("You are not authorized to delete this comment.");

            context.RequestComments.Remove(comment);
            await context.SaveChangesAsync();

            await auditLogService.LogCommentRemovedAsync(comment.RequestId, userId, comment.Content);
        }

        public async Task<CommentDto> UpdateCommentAsync(string userId, int commentId, UpdateCommentDto commentDto)
        {
            var comment = await context.RequestComments.FindAsync(commentId);

            if (comment == null)
                throw new NotFoundException($"Comment with ID {commentId} not found.");

            if (comment.UserId != userId)
                throw new UnauthorizedException("You are not authorized to update this comment.");

            var oldContent = comment.Content;

            if (oldContent != commentDto.Content)
            {
                comment.Content = commentDto.Content;
                await context.SaveChangesAsync();
                await auditLogService.LogCommentUpdatedAsync(comment.RequestId, userId, comment.Content, oldContent);
            }

            return mapper.MapToDto(comment);
        }
    }
}
