namespace API.Controllers
{
    using System.Security.Claims;

    using API.DTOs.Requests;
    using API.Models.Enums;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/requests")]
    public class RequestsController(IRequestService requestService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto createRequestDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.CreateRequest(userId, createRequestDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests(
            [FromQuery] string? userId,
            [FromQuery] Status? status,
            [FromQuery] RequestType? type,
            [FromQuery] bool includeDeleted = false,
            [FromQuery] int? take = null,
            [FromQuery] string? search = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdminOrManager = User.IsInRole("Manager") || User.IsInRole("Admin");

            if (!string.IsNullOrEmpty(userId))
            {
                if (!isAdminOrManager && userId != currentUserId)
                {
                    return Forbid();
                }

                return Ok(await requestService.GetAllRequests(userId, status, type, includeDeleted, take, search));
            }

            if (!isAdminOrManager)
            {
                return Ok(await requestService.GetAllRequests(currentUserId, status, type, includeDeleted, take, search));
            }

            return Ok(await requestService.GetAllRequests(null, status, type, includeDeleted, take, search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            return Ok(await requestService.GetRequestById(id));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] JsonPatchDocument<UpdateRequestDto> patchDoc)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.UpdateRequest(userId, id, patchDoc));
        }

        [Authorize(Policy = "ManagerAccess")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.ApproveRequest(userId, id));
        }

        [Authorize(Policy = "ManagerAccess")]
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.RejectRequest(userId, id));
        }

        [Authorize(Policy = "ManagerAccess")]
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteRequest(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.CompleteRequest(userId, id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await requestService.DeleteRequest(userId, id);
            return NoContent();
        }

        [Authorize(Policy = "AdminAccess")]
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreRequest(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await requestService.RestoreRequest(userId, id));
        }

        [HttpGet("request-types")]
        public IActionResult GetRequestTypes()
        {
            var types = Enum.GetValues<RequestType>()
                .Select(t => t.ToString());

            return Ok(types);
        }
    }
}
