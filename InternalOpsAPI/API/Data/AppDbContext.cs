namespace API.Data
{
    using API.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Request>(entity =>
            {
                entity.Property(r => r.RequestType)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(r => r.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.IsDeleted);
                entity.HasIndex(e => e.RequestType);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => new { e.Status, e.UpdatedAt });
                entity.HasIndex(e => e.UpdatedAt);

                entity
                   .HasOne(r => r.RequestedBy)
                   .WithMany(u => u.RequestedRequests)
                   .HasForeignKey(r => r.RequestedById)
                   .OnDelete(DeleteBehavior.Restrict);

                entity
                   .HasOne(r => r.HandledBy)
                   .WithMany(u => u.HandledRequests)
                   .HasForeignKey(r => r.HandledById)
                   .OnDelete(DeleteBehavior.Restrict);

                entity
                   .HasOne(r => r.DeletedBy)
                   .WithMany()
                   .HasForeignKey(r => r.DeletedById)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.Comments)
                   .WithOne(c => c.Request)
                   .HasForeignKey(c => c.RequestId)
                   .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(r => r.Attachments)
                   .WithOne(a => a.Request)
                   .HasForeignKey(a => a.RequestId)
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(n => n.Type)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RelatedRequestId);

                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.RelatedRequest)
                    .WithMany()
                    .HasForeignKey(n => n.RelatedRequestId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<RequestComment>(entity =>
            {
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(a => a.Action)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.HasOne(a => a.ChangedBy)
                    .WithMany(u => u.AuditLogs)
                    .HasForeignKey(a => a.ChangedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.RequestId);
                entity.HasIndex(e => e.ChangedById);
                entity.HasIndex(e => e.Action);
                entity.HasIndex(e => e.Timestamp);
            });
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<RequestComment> RequestComments { get; set; }

        public DbSet<RequestAttachment> RequestAttachments { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
