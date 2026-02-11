namespace API.Services
{
    using System.Threading.Tasks;

    using API.Data;
    using API.DTOs;
    using API.DTOs.Attachments;
    using API.DTOs.Comments;
    using API.DTOs.Requests;
    using API.Exceptions;
    using API.Models;
    using API.Models.Enums;
    using API.Services.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class RequestService(AppDbContext context) : IRequestService
    {
        public Task<RequestDto> CreateRequest(string userId, CreateRequestDto RequestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RequestDto>> GetAllRequests(string? userId = null, Status? status = null, RequestType? type = null, bool includeDeleted = false)
        {
            var query = context.Requests
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(r => r.RequestedById == userId);
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status);
            }

            if (type.HasValue)
            {
                query = query.Where(r => r.RequestType == type);
            }

            if (!includeDeleted)
            {
                query = query.Where(r => !r.IsDeleted);
            }

            return await query
                .Select(r => MapToDto(r))
                .ToListAsync();
        }

        public async Task<List<RequestDto>> GetPendingRequests() => await context.Requests
            .AsNoTracking()
            .Where(r => r.Status == Status.Pending && !r.IsDeleted)
            .Select(r => MapToDto(r))
            .ToListAsync();

        public async Task<RequestDetailDto> GetRequest(int requestId)
        {
            var query = context.Requests
                .AsNoTracking()
                .Where(r => r.Id == requestId && !r.IsDeleted)
                .Select(r => MapToDetailDto(r));

            if (!query.Any())
            {
                throw new BadRequestException("Request not found.");
            }

            return await query.FirstAsync();
        }

        public Task<RequestDto> UpdateRequest(string userId, int requestId, UpdateRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestDto> ApproveRequest(string userId, int requestId)
        {
            var request = await context.FindAsync<Request>(requestId);

            if (request == null || request.IsDeleted)
            {
                throw new BadRequestException("Request not found.");
            }

            if (request.Status != Status.Pending)
            {
                throw new BadRequestException("Only pending requests can be approved.");
            }

            request.Status = Status.Approved;
            request.HandledById = userId;
            await context.SaveChangesAsync();

            return MapToDto(request);
        }

        public Task<RequestDto> RejectRequest(string userId, int requestId)
        {
            throw new NotImplementedException();
        }

        public Task<RequestDto> CompleteRequest(string userId, int requestId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequest(string userId, int requestId)
        {
            throw new NotImplementedException();
        }

        public Task<RequestDto> RestoreRequest(string userId, int requestId)
        {
            throw new NotImplementedException();
        }

        private static RequestDto MapToDto(Request request)
        {
            return new RequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                RequestType = request.RequestType,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                RequestedBy = new UserDto
                {
                    Id = request.RequestedBy.Id,
                    Email = request.RequestedBy.Email!
                },
                HandledBy = request.HandledBy != null
                    ? new UserDto
                    {
                        Id = request.HandledBy.Id,
                        Email = request.HandledBy.Email!
                    }
                    : null
            };
        }

        private static RequestDetailDto MapToDetailDto(Request request)
        {
            return new RequestDetailDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                RequestType = request.RequestType,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                RequestedBy = new UserDto
                {
                    Id = request.RequestedBy.Id,
                    Email = request.RequestedBy.Email!
                },
                HandledBy = request.HandledBy != null
                    ? new UserDto
                    {
                        Id = request.HandledBy.Id,
                        Email = request.HandledBy.Email!
                    }
                    : null,
                Comments = [.. request.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Author = new UserDto
                    {
                        Id = c.UserId!,
                        Email = c.User!.Email!
                    }
                })],
                Attachments = [.. request.Attachments.Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileUrl = a.FileUrl
                })]
            };
        }
    }
}