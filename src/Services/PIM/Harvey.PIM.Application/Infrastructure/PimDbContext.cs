using System.ComponentModel.DataAnnotations.Schema;
using Harvey.Domain;
using Harvey.PIM.Application.FieldFramework.Entities;
using Harvey.PIM.Application.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harvey.PIM.Application.Infrastructure
{
    public class PimDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Assortment> Assortments { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldTemplate> FieldTemplates { get; set; }
        public DbSet<Field_FieldTemplate> Field_FieldTemplates { get; set; }
        public DbSet<FieldValue> FieldValues { get; set; }
        public DbSet<EntityRef> EntityRefs { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<AssortmentAssignment> AssortmentAssignments { get; set; }
        public DbSet<ChannelAssignment> ChannelAssignments { get; set; }
        public DbSet<Price> Prices { get; set; }

        public PimDbContext(DbContextOptions<PimDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Setup(modelBuilder.Entity<Assortment>());
            Setup(modelBuilder.Entity<Category>());
            Setup(modelBuilder.Entity<Field>());
            Setup(modelBuilder.Entity<FieldValue>());
            Setup(modelBuilder.Entity<FieldTemplate>());
            Setup(modelBuilder.Entity<Field_FieldTemplate>());
            Setup(modelBuilder.Entity<EntityRef>());
            Setup(modelBuilder.Entity<AppSetting>());
            Setup(modelBuilder.Entity<Brand>());
            Setup(modelBuilder.Entity<Location>());
            Setup(modelBuilder.Entity<Product>());
            Setup(modelBuilder.Entity<Variant>());
            Setup(modelBuilder.Entity<Channel>());
            Setup(modelBuilder.Entity<Price>());
            Setup(modelBuilder.Entity<AssortmentAssignment>());
            Setup(modelBuilder.Entity<ChannelAssignment>());
        }

        public void Setup(EntityTypeBuilder<Assortment> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
        }
        public void Setup(EntityTypeBuilder<Category> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
        }

        public void Setup(EntityTypeBuilder<Field> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);

            entityConfig.HasMany(x => x.FieldValues)
                .WithOne(x => x.Field)
                .HasForeignKey(x => x.FieldId);
        }

        public void Setup(EntityTypeBuilder<FieldValue> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);

            entityConfig.HasOne(x => x.Field)
                .WithMany(x => x.FieldValues)
                .HasForeignKey(x => x.FieldId);
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
        }

        public void Setup(EntityTypeBuilder<FieldTemplate> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);

            entityConfig.HasIndex(x => x.Name).IsUnique();
        }

        public void Setup(EntityTypeBuilder<Field_FieldTemplate> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);

            entityConfig.
                HasIndex(x => new { x.FieldId, x.FieldTemplateId }).IsUnique();
        }

        public void Setup(EntityTypeBuilder<EntityRef> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
        }

        public void Setup(EntityTypeBuilder<AppSetting> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
        }

        public void Setup(EntityTypeBuilder<Brand> entityConfig)
        {
            entityConfig.HasIndex(x => x.Name).IsUnique();
        }

        public void Setup(EntityTypeBuilder<Location> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => new { x.Name, x.Type }).IsUnique();
        }

        public void Setup(EntityTypeBuilder<Product> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
        }

        public void Setup(EntityTypeBuilder<Channel> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
            entityConfig.HasIndex(x => x.ServerInformation).IsUnique();
        }

        public void Setup(EntityTypeBuilder<Variant> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
        }

        public void Setup(EntityTypeBuilder<Price> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.ToTable("Price");
            entityConfig
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
        }

        public void Setup(EntityTypeBuilder<AssortmentAssignment> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => new { x.AssortmentId, x.ReferenceId, x.EntityType }).IsUnique();
        }

        public void Setup(EntityTypeBuilder<ChannelAssignment> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => new { x.ChannelId, x.ReferenceId, x.EntityType }).IsUnique();
        }
    }

    public class TransientPimDbContext : PimDbContext
    {
        public TransientPimDbContext(DbContextOptions<PimDbContext> options) : base(options)
        {
        }
    }
}
