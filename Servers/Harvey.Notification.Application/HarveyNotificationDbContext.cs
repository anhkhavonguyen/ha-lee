using Harvey.Notification.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Notification.Api
{
    public class HarveyNotificationDbContext : DbContext
    {
        public DbSet<ErrorLogEntry> ErrorLogEntries { get; set; }
        public DbSet<ErrorLogSource> ErrorLogSources { get; set; }
        public DbSet<NotificationStatus> NotificationStatus { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Application.Entities.Notification> Notifications { get; set; }
        public DbSet<Template> Templates { get; set; }

        public HarveyNotificationDbContext(DbContextOptions<HarveyNotificationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ErrorLogSource>()
               .HasKey(a => a.Id);

            modelBuilder.Entity<ErrorLogSource>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<ErrorLogEntry>()
                        .Ignore(t => t.UpdatedBy)
                        .Ignore(t => t.UpdatedDate)
                        .HasKey(k => k.Id);

            modelBuilder.Entity<ErrorLogEntry>()
                    .HasOne(d => d.ErrorLogSource)
                    .WithMany(a => a.ErrorLogEntries)
                    .HasForeignKey(a => a.ErrorLogSourceId);

            modelBuilder.Entity<NotificationStatus>()
                        .Ignore(t => t.CreatedBy)
                        .Ignore(t => t.CreatedDate)
                        .Ignore(t => t.UpdatedBy)
                        .Ignore(t => t.UpdatedDate)
                        .HasKey(k => k.Id);
            modelBuilder.Entity<NotificationStatus>()
                        .Property(p => p.Id)
                        .ValueGeneratedNever();
            modelBuilder.Entity<NotificationStatus>()
                        .ToTable("NotificationStatus");

            modelBuilder.Entity<Application.Entities.Notification>()
                        .HasKey(k => k.Id);

            modelBuilder.Entity<NotificationType>()
                        .Ignore(t => t.CreatedBy)
                        .Ignore(t => t.CreatedDate)
                        .Ignore(t => t.UpdatedBy)
                        .Ignore(t => t.UpdatedDate)
                        .HasKey(k => k.Id);
            modelBuilder.Entity<NotificationType>()
                        .Property(p => p.Id)
                        .ValueGeneratedNever();

            modelBuilder.Entity<Template>()
                        .HasKey(k => k.Id);
        }
    }
}
