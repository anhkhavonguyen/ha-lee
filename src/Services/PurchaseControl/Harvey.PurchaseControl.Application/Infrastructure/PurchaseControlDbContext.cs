using Harvey.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harvey.PurchaseControl.Application.Infrastructure
{
    public class PurchaseControlDbContext : DbContext
    {
        public DbSet<AppSetting> AppSettings { get; set; }
        public PurchaseControlDbContext(DbContextOptions<PurchaseControlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Setup(modelBuilder.Entity<AppSetting>());
        }

        public void Setup(EntityTypeBuilder<AppSetting> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
        }
    }
}
