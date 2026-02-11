namespace API.DTOs.Requests
{
    using API.DTOs.Attachments;
    using API.DTOs.AuditLogs;
    using API.DTOs.Comments;

    public class RequestDetailDto : RequestDto
    {
        public List<CommentDto> Comments { get; set; } = [];
        public List<AttachmentDto> Attachments { get; set; } = [];
        public List<AuditLogDto> AuditLogs { get; set; } = [];
    }
}
