namespace API.Controllers
{
    using API.DTOs.AuditLogs;
    using API.Models.Enums;
    using API.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize(Policy = "ManagerAccess")]
    [Route("api/logs")]
    public class AuditLogController(IAuditLogService auditLogService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] AuditFilterDto filter)
        {
            var logs = await auditLogService.GetAllAuditLogs(filter);
            return Ok(logs);
        }

        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetAuditLogsByRequest(int requestId)
        {
            var logs = await auditLogService.GetAllAuditLogs(new AuditFilterDto { RequestId = requestId });
            return Ok(logs);
        }

        [HttpGet("actions")]
        public IActionResult GetAuditActions()
        {
            var actions = Enum.GetValues<AuditAction>()
                .Select(s => new
                {
                    value = s.ToString().ToLower(),
                    label = s.ToString()
                });

            return Ok(actions);
        }
    }
}
