namespace API.Controllers
{
    using API.Models.Enums;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize]
    [Route("api/logs")]
    public class AuditLogController(IAuditLogService auditLogService) : ControllerBase
    {
        [Authorize(Policy = "ManagerAccess")]
        [HttpGet]
        public async Task<IActionResult> GetAuditLogs(
            [FromQuery] int? requestId = null,
            [FromQuery] string? userId = null,
            [FromQuery] AuditAction? action = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var logs = await auditLogService.GetAllAuditLogs(requestId, userId, action, fromDate, toDate);
            return Ok(logs);
        }

        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetAuditLogsByRequest(int requestId)
        {
            var logs = await auditLogService.GetAuditLogsByRequest(requestId);
            return Ok(logs);
        }

        [HttpGet("recent")]
        [Authorize(Policy = "ManagerAccess")]
        public async Task<IActionResult> GetRecentAuditLogs(
            [FromQuery] int hours = 24)
        {
            var logs = await auditLogService.GetAllAuditLogs(fromDate: DateTime.UtcNow.AddHours(-hours));
            return Ok(logs);
        }
    }
}
