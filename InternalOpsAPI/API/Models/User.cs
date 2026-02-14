namespace API.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        public ICollection<Request> RequestedRequests { get; set; } = [];

        public ICollection<Request> HandledRequests { get; set; } = [];

        public ICollection<RequestComment> Comments { get; set; } = [];

        public ICollection<AuditLog> AuditLogs { get; set; } = [];

        public ICollection<Notification> Notifications { get; set; } = [];
    }
}
