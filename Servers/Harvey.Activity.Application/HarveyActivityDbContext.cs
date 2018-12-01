using Harvey.Activity.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harvey.Activity.Api
{
    public class HarveyActivityDbContext : DbContext
    {
        public HarveyActivityDbContext(DbContextOptions<HarveyActivityDbContext> options) : base(options)
        {
        }

        public DbSet<ActionActivity> Activities { get; set; }
        public DbSet<AreaActivity> AreaActivities { get; set; }
        public DbSet<ActionType> ActionTypies { get; set; }
        public DbSet<ErrorLogEntry> ErrorLogEntries { get; set; }
        public DbSet<ErrorLogSource> ErrorLogSources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActionActivity>().HasKey(p => p.Id);

            modelBuilder.Entity<ActionActivity>()
               .HasOne(p => p.AreaActivity)
               .WithMany(i => i.ActionActivities)
               .HasForeignKey(b => b.ActionAreaId);

            modelBuilder.Entity<ActionActivity>()
               .HasOne(p => p.ActionType)
               .WithMany(i => i.ActionActivities)
               .HasForeignKey(b => b.ActionTypeId);

            modelBuilder.Entity<ErrorLogSource>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<ErrorLogSource>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<ErrorLogEntry>()
               .HasKey(a => a.Id);

            modelBuilder.Entity<ErrorLogEntry>()
                    .HasOne(d => d.ErrorLogSource)
                    .WithMany(a => a.ErrorLogEntries)
                    .HasForeignKey(a => a.ErrorSourceId);
        }
    }
}
