namespace API.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using API.Data;
    using API.DTOs.Paging;
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
                await notificationService.SendNotificationAsync(manager.Id, $"New {request.RequestType} request submitted: {request.Title}", request.Id, NotificationType.RequestCreated);
            }

            return mapper.MapToDto(request);
        }

        public async Task<PagedResponse<RequestDto>> GetAllRequests(ClaimsPrincipal user, RequestFilterDto filter)
        {
            var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdminOrManager = user.IsInRole("Manager") || user.IsInRole("Admin");

            string? effectiveUserId = null;
            var createdFrom = filter.CreatedFrom?.ToDateTime(TimeOnly.MinValue);
            var createdTo = filter.CreatedTo?.ToDateTime(TimeOnly.MinValue).AddDays(1);

            var handledFrom = filter.HandledFrom?.ToDateTime(TimeOnly.MinValue);
            var handledTo = filter.HandledTo?.ToDateTime(TimeOnly.MinValue).AddDays(1);

            var effectiveIncludeDeleted = isAdminOrManager && filter.IncludeDeleted;

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                effectiveUserId = isAdminOrManager || filter.UserId == currentUserId
                    ? filter.UserId
                    : currentUserId;
            }
            else if (!isAdminOrManager)
            {
                effectiveUserId = currentUserId;
            }

            var query = context.Requests.Include(r => r.DeletedBy).AsNoTracking();

            if (!string.IsNullOrEmpty(effectiveUserId))
                query = query.Where((r) => r.RequestedById == effectiveUserId);

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(r =>
                r.Title.Contains(filter.Search) ||
                (r.Description != null && r.Description.Contains(filter.Search)) ||
                    (r.RequestedBy != null && r.RequestedBy.Email != null && r.RequestedBy.Email.Contains(filter.Search)));

            if (filter.Status.HasValue)
                query = query.Where((r) => r.Status == filter.Status);

            if (filter.Type.HasValue)
                query = query.Where((r) => r.RequestType == filter.Type);

            if (!effectiveIncludeDeleted)
                query = query.Where((r) => !r.IsDeleted);

            if (createdFrom.HasValue)
                query = query.Where((r) => r.CreatedAt >= createdFrom);

            if (createdTo.HasValue)
                query = query.Where((r) => r.CreatedAt < createdTo);

            if (handledFrom.HasValue)
                query = query.Where((r) => r.UpdatedAt >= handledFrom && r.Status != Status.Pending);

            if (handledTo.HasValue)
                query = query.Where((r) => r.UpdatedAt < handledTo && r.Status != Status.Pending);

            query = query.OrderByDescending((r) => r.CreatedAt);


            var projected = mapper.ProjectToDto(query);

            return await projected.ToPagedResponseAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<RequestDetailDto> GetRequestById(int requestId)
        {
            var user = httpContextAccessor.HttpContext?.User;
            bool isAdminOrManager = user!.IsInRole("Manager") || user.IsInRole("Admin");

            var request = await context.Requests
              .AsNoTracking()
              .AsSplitQuery()
              .Where(r => r.Id == requestId && (isAdminOrManager || !r.IsDeleted))
              .Include(r => r.RequestedBy)
              .Include(r => r.HandledBy)
              .Include(r => r.DeletedBy)
              .Include(r => r.Comments)
                .ThenInclude(c => c.User)
              .Include(r => r.Attachments)
              .Include(r => r.AuditLogs.OrderByDescending(a => a.Timestamp))
                .ThenInclude(a => a.ChangedBy)
              .FirstOrDefaultAsync();

            if (request == null)
                throw new NotFoundException($"Request with id {requestId} not found.");

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

            if (request.Status == Status.Approved || request.Status == Status.Completed)
                throw new BadRequestException("Approved or completed requests cannot be updated.");

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

            if (request.Status == Status.Approved || request.Status == Status.Completed)
                throw new BadRequestException("Approved or completed requests cannot be deleted.");

            var user = httpContextAccessor.HttpContext?.User;

            if (!(await authorizationService.AuthorizeAsync(user!, request.RequestedById, "OwnerOrManager")).Succeeded)
                throw new UnauthorizedException("You do not have permission to access this request.");


            await attachmentService.DeleteAttachmentsForRequestAsync(userId, request);

            request.IsDeleted = true;
            request.DeletedAt = DateTime.UtcNow;
            request.DeletedById = userId;

            await context.SaveChangesAsync();
            await auditLogService.LogDeletedAsync(requestId, userId, request.Title);
        }

        public async Task<RequestDto> RestoreRequest(string userId, int requestId)
        {
            var request = await context.Requests.FirstOrDefaultAsync((Request r) => r.Id == requestId && r.IsDeleted);

            if (request == null)
                throw new NotFoundException($"Request with id {requestId} not found or is not deleted.");

            request.IsDeleted = false;
            request.DeletedAt = null;
            request.DeletedById = null;

            await context.SaveChangesAsync();
            await auditLogService.LogRestoredAsync(requestId, userId, request.Title);
            return mapper.MapToDto(request);
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