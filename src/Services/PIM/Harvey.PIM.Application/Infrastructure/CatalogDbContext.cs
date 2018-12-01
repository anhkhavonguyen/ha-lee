using Harvey.PIM.Application.Infrastructure.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvey.PIM.Application.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<CatalogCategory> Categories { get; set; }
        public DbSet<CatalogProduct> Products { get; set; }
        public DbSet<CatalogVariant> Variants { get; set; }
        public DbSet<CatalogFieldValue> FieldValues { get; set; }
        public DbSet<CatalogPrice> Prices { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Setup(modelBuilder.Entity<CatalogCategory>());
            Setup(modelBuilder.Entity<CatalogProduct>());
            Setup(modelBuilder.Entity<CatalogVariant>());
            Setup(modelBuilder.Entity<CatalogPrice>());

        }

        public void Setup(EntityTypeBuilder<CatalogCategory> entityConfig)
        {
            entityConfig.ToTable("Categories");
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
            entityConfig
               .Property(x => x.Id)
               .ValueGeneratedNever()
               .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            entityConfig.Ignore(x => x.CorrelationId);
        }

        public void Setup(EntityTypeBuilder<CatalogProduct> entityConfig)
        {
            entityConfig.ToTable("Products");
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            entityConfig.Ignore(x => x.CorrelationId);
        }

        public void Setup(EntityTypeBuilder<CatalogVariant> entityConfig)
        {
            entityConfig.ToTable("Variants");
            entityConfig.HasKey(x => x.Id);
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            entityConfig.Ignore(x => x.CorrelationId);
        }

        public void Setup(EntityTypeBuilder<CatalogFieldValue> entityConfig)
        {
            entityConfig.ToTable("FieldValues");
            entityConfig.HasKey(x => x.Id);
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            entityConfig.Ignore(x => x.CorrelationId);
        }

        public void Setup(EntityTypeBuilder<CatalogPrice> entityConfig)
        {
            entityConfig.ToTable("Prices");
            entityConfig.HasKey(x => x.Id);
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            entityConfig.Ignore(x => x.CorrelationId);
        }
    }
}
