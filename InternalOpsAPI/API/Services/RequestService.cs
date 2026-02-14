namespace API.Services
{
    using System.Threading.Tasks;

    using API.Data;
    using API.DTOs.Requests;
    using API.Exceptions;
    using API.Mappers;
    using API.Models;
    using API.Models.Enums;
    using API.Services.Extensions;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
    using Microsoft.EntityFrameworkCore;

    public class RequestService(AppDbContext context, RequestMapper mapper, IAuditLogService auditLogService, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IAttachmentService attachmentService, INotificationService notificationService, UserManager<User> userManager) : IRequestService
    {
        public async Task<RequestDto> CreateRequest(string userId, CreateRequestDto requestDto)
        {
            var request = new Request
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                RequestType = requestDto.RequestType,
                Status = Status.Pending,
                RequestedById = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Add(request);
            await context.SaveChangesAsync();
            await context.Entry(request).Reference(r => r.RequestedBy).LoadAsync();

            await auditLogService.LogCreatedAsync(request.Id, userId, request.Title);

            foreach (var manager in await userManager.GetUsersInRoleAsync("Manager"))
            {
                await notificationService.SendNotificationAsync(manager.Id, $"New ${request.RequestType} submitted: {request.Title}", request.Id, NotificationType.RequestCreated);
            }

            return mapper.MapToDto(request);
        }

        public async Task<List<RequestDto>> GetAllRequests(string? userId = null, Status? status = null, RequestType? type = null, bool includeDeleted = false)
        {
            var query = context.Requests.AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where((r) => r.RequestedById == userId);
            }

            if (status.HasValue)
            {
                query = query.Where((r) => (int?)r.Status == (int?)status);
            }

            if (type.HasValue)
            {
                query = query.Where((r) => (int?)r.RequestType == (int?)type);
            }

            if (!includeDeleted)
            {
                query = query.Where((r) => !r.IsDeleted);
            }

            return await mapper.ProjectToDto(query).ToListAsync();
        }

        public async Task<List<RequestDto>> GetPendingRequests()
        {
            return await mapper.ProjectToDto(context.Requests
                .AsNoTracking()
                .Where(r => r.Status == Status.Pending && !r.IsDeleted))
                .ToListAsync();
        }

        public async Task<RequestDetailDto> GetRequestById(int requestId)
        {
            var request = await context.Requests
              .AsNoTracking()
              .Where(r => r.Id == requestId && !r.IsDeleted)
              .Include(r => r.RequestedBy)
              .Include(r => r.HandledBy)
              .Include(r => r.Comments)
                .ThenInclude(c => c.User)
              .Include(r => r.Attachments)
              .Include(r => r.AuditLogs)
              .FirstOrDefaultAsync();

            if (request == null)
                throw new NotFoundException($"Request with id {requestId} not found.");

            var user = httpContextAccessor.HttpContext?.User;

            if (!(await authorizationService.AuthorizeAsync(user!, request.RequestedById, "OwnerOrManager")).Succeeded)
                throw new UnauthorizedException("You do not have permission to access this request.");

            return mapper.MapToDetailDto(request);
        }

        public async Task<RequestDto> UpdateRequest(string userId, int requestId, JsonPatchDocument<UpdateRequestDto> patchDoc)
        {
            var request = await context.Requests
                 .Where(r => r.Id == requestId && !r.IsDeleted)
                 .Include(r => r.RequestedBy)
                 .Include(r => r.HandledBy)
                 .FirstOrDefaultAsync();

            if (request == null)
                throw new NotFoundException($"Request with id {requestId} not found.");

            var user = httpContextAccessor.HttpContext?.User;
            if (!(await authorizationService.AuthorizeAsync(user!, request.RequestedById, "OwnerOrManager")).Succeeded)
                throw new UnauthorizedException("You do not have permission to update this request.");

            var originalTitle = request.Title;
            var originalDescription = request.Description;
            var originalRequestType = request.RequestType;

            var updateDto = new UpdateRequestDto
            {
                Title = request.Title,
                Description = request.Description,
                RequestType = request.RequestType
            };

            patchDoc.ApplyTo(updateDto);

            if (updateDto.Title != null && (updateDto.Title.Length < 5 || updateDto.Title.Length > 100))
                throw new BadRequestException("Title must be between 5 and 100 characters");

            if (updateDto.Description != null && updateDto.Description.Length > 500)
                throw new BadRequestException("Description cannot exceed 500 characters.");

            request.Title = updateDto.Title ?? request.Title;
            request.Description = updateDto.Description;
            request.RequestType = updateDto.RequestType ?? request.RequestType;
            request.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            var fieldChanges = new Dictionary<string, (string OldValue, string NewValue)>
            {
                ["Title"] = (originalTitle, request.Title),
                ["Description"] = (originalDescription!, request.Description!),
                ["RequestType"] = (originalRequestType.ToString(), request.RequestType.ToString())
            };

            await auditLogService.LogFieldChangesAsync(requestId, userId, fieldChanges!);

            return mapper.MapToDto(request);
        }
        public async Task<RequestDto> ApproveRequest(string userId, int requestId) =>
            await UpdateRequestStatus(userId, requestId, Status.Pending, Status.Approved);

        public async Task<RequestDto> RejectRequest(string userId, int requestId) =>
            await UpdateRequestStatus(userId, requestId, Status.Pending, Status.Rejected);

        public async Task<RequestDto> CompleteRequest(string userId, int requestId) =>
            await UpdateRequestStatus(userId, requestId, Status.Approved, Status.Completed);

        public async Task DeleteRequest(string userId, int requestId)
        {
            var request = await context.Requests
                .Include((r) => r.Attachments)
                .FirstOrDefaultAsync((r) => r.Id == requestId && !r.IsDeleted);

            if (request == null)
                throw new NotFoundException($"Request with id {requestId} not found.");

            if (request.IsDeleted)
                throw new BadRequestException("Request is already deleted.");

            if (request.Status == Status.Approved || request.Status == Status.Completed)
                throw new BadRequestException("Approved or completed requests cannot be deleted.");

            var user = httpContextAccessor.HttpContext?.User;

            if (!(await authorizationService.AuthorizeAsync(user!, request.RequestedById, "OwnerOrManager")).Succeeded)
                throw new UnauthorizedException("You do not have permission to access this request.");

            foreach (var attachment in request.Attachments)
            {
                await attachmentService.DeleteAttachmentAsync(userId, attachment.Id);
            }

            request.IsDeleted = true;
            request.DeletedAt = DateTime.UtcNow;
            request.DeletedById = userId;

            await context.SaveChangesAsync();
            await auditLogService.LogDeletedAsync(requestId, userId, request.Title);
        }

        public Task<RequestDto> RestoreRequest(string userId, int requestId)
        {
            throw new NotImplementedException();
        }

        private async Task<RequestDto> UpdateRequestStatus(string userId, int requestId, Status requiredStatus, Status newStatus)
        {
            var request = await context.Requests
                .Include((r) => r.RequestedBy)
                .Include((r) => r.HandledBy)
                .FirstOrDefaultAsync((r) => r.Id == requestId && !r.IsDeleted);

            if (request == null)
                throw new NotFoundException($"Request with id {requestId} not found.");

            if (request.Status != requiredStatus)
                throw new BadRequestException("Only " + requiredStatus.ToString().ToLower() + " requests can be completed.");

            request.Status = newStatus;
            request.HandledById = userId;
            request.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            await auditLogService.LogStatusChangedAsync(requestId, userId, requiredStatus, newStatus);

            await notificationService.SendNotificationAsync(
                request.RequestedById,
                $"Your {request.RequestType} '{request.Title}' has been {newStatus.ToString().ToLower()}.",
                request.Id,
                NotificationType.RequestStatusChanged);

            return mapper.MapToDto(request);
        }
    }
}