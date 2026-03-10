namespace API.Services.Interfaces
{
    using System.Security.Claims;

    using API.DTOs.Paging;
    using API.DTOs.Requests;

    using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

    public interface IRequestService
    {
        public Task<RequestDto> CreateRequest(string userId, CreateRequestDto RequestDto);

        Task<PagedResponse<RequestDto>> GetAllRequests(ClaimsPrincipal user, RequestFilterDto filter);
        public Task<RequestDetailDto> GetRequestById(int requestId);

        public Task<RequestDto> UpdateRequest(string userId, int requestId, JsonPatchDocument<UpdateRequestDto> patchDoc);

        public Task<RequestDto> ApproveRequest(string userId, int requestId);
        public Task<RequestDto> RejectRequest(string userId, int requestId);
        public Task<RequestDto> CompleteRequest(string userId, int requestId);


        public Task DeleteRequest(string userId, int requestId);
        public Task<RequestDto> RestoreRequest(string userId, int requestId);
    }
}
