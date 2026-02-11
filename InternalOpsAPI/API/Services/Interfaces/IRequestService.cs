namespace API.Services.Interfaces
{
    using API.DTOs.Requests;
    using API.Models.Enums;

    public interface IRequestService
    {
        public Task<RequestDto> CreateRequest(string userId, CreateRequestDto RequestDto);

        Task<List<RequestDto>> GetAllRequests(string? userId = null, Status? status = null, RequestType? type = null, bool includeDeleted = false);
        public Task<RequestDetailDto> GetRequest(int requestId);
        public Task<List<RequestDto>> GetPendingRequests();

        public Task<RequestDto> UpdateRequest(string userId, int requestId, UpdateRequestDto requestDto);

        public Task<RequestDto> ApproveRequest(string userId, int requestId);
        public Task<RequestDto> RejectRequest(string userId, int requestId);
        public Task<RequestDto> CompleteRequest(string userId, int requestId);


        public Task DeleteRequest(string userId, int requestId);
        public Task<RequestDto> RestoreRequest(string userId, int requestId);
    }
}
