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

            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestedBy)
                .WithMany(u => u.RequestedRequests)
                .HasForeignKey(r => r.RequestedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
               .HasOne(r => r.HandledBy)
               .WithMany(u => u.HandledRequests)
               .HasForeignKey(r => r.HandledById)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.DeletedBy)
                .WithMany()
                .HasForeignKey(r => r.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.ChangedBy)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(a => a.ChangedById)
            .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<RequestComment> RequestComments { get; set; }

        public DbSet<RequestAttachment> RequestAttachments { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
