namespace API.Services.Extensions
{
    using API.Models.Enums;
    using API.Services.Interfaces;

    public static class AuditLogServiceExtensions
    {
        public static Task LogCreatedAsync(this IAuditLogService auditLogService, int requestId, string userId, string title)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.Created, summary: $"Request '{title}' created");
        }

        public static async Task LogFieldChangesAsync(this IAuditLogService auditLogService, int requestId, string userId, Dictionary<string, (string? OriginalValue, string? NewValue)> changes)
        {
            var tasks = new List<Task>();

            foreach (var (fieldName, (originalValue, newValue)) in changes)
            {
                if (originalValue != newValue)
                {
                    var summary = string.IsNullOrEmpty(originalValue) ? $"{fieldName} set to '{newValue}'" :
                                  string.IsNullOrEmpty(newValue) ? $"{fieldName} cleared (was '{originalValue}')" :
                                  $"{fieldName} changed from '{originalValue}' to '{newValue}'";

                    tasks.Add(auditLogService.LogAsync(requestId, userId, AuditAction.Updated, summary, oldValue: originalValue, newValue: newValue));
                }
            }

            if (tasks.Count != 0)
            {
                await Task.WhenAll(tasks);
            }
        }

        public static Task LogDeletedAsync(this IAuditLogService auditLogService, int requestId, string userId, string title)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.Deleted, $"Request '{title}' deleted");
        }

        public static Task LogRestoredAsync(this IAuditLogService auditLogService, int requestId, string userId, string title)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.Restored, $"Request '{title}' restored");
        }

        public static Task LogStatusChangedAsync(this IAuditLogService auditLogService, int requestId, string userId, Status oldStatus, Status newStatus)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.StatusChanged, summary: $"Status changed from {oldStatus} to {newStatus}", oldStatus: oldStatus, newStatus: newStatus);
        }

        public static Task LogCommentAddedAsync(this IAuditLogService auditLogService, int requestId, string userId, string commentContent)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.CommentAdded, summary: $"Comment added: {commentContent}");
        }

        public static Task LogCommentRemovedAsync(this IAuditLogService auditLogService, int requestId, string userId, string commentContent)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.CommentRemoved, summary: $"Comment removed: {commentContent}");
        }

        public static Task LogCommentUpdatedAsync(this IAuditLogService auditLogService, int requestId, string userId, string newCommentContent, string oldCommentContent)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.CommentUpdated, summary: $"Comment updated from '{oldCommentContent}' to '{newCommentContent}'", oldValue: oldCommentContent, newValue: newCommentContent);
        }

        public static Task LogAttachmentAddedAsync(this IAuditLogService auditLogService, int requestId, string userId, string fileName)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.AttachmentAdded, summary: $"Attachment added: {fileName}");
        }

        public static Task LogAttachmentRemovedAsync(this IAuditLogService auditLogService, int requestId, string userId, string fileName)
        {
            return auditLogService.LogAsync(requestId, userId, AuditAction.AttachmentRemoved, summary: $"Attachment removed: {fileName}");
        }
    }
}
