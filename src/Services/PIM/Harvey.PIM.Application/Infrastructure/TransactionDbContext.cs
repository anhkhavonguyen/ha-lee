using Harvey.PIM.Application.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harvey.PIM.Application.Infrastructure
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<GIWDocument> GIWDocuments { get; set; }
        public DbSet<GIWDocumentItem> GIWDocumentItems { get; set; }
        public DbSet<StockType> StockTypes { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Setup(modelBuilder.Entity<InventoryTransaction>());
            Setup(modelBuilder.Entity<StockTransaction>());
            Setup(modelBuilder.Entity<GIWDocument>());
            Setup(modelBuilder.Entity<GIWDocumentItem>());
            Setup(modelBuilder.Entity<StockType>());
            Setup(modelBuilder.Entity<TransactionType>());
        }

        public void Setup(EntityTypeBuilder<InventoryTransaction> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.Property(x => x.GIWDocumentId).IsRequired();
            entityConfig.HasOne<TransactionType>()
              .WithMany()
              .HasForeignKey(x => x.TransactionTypeId);
            entityConfig.HasOne<GIWDocument>()
                .WithMany()
                .HasForeignKey(x => x.GIWDocumentId);
        }

        public void Setup(EntityTypeBuilder<StockTransaction> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.Property(x => x.VariantId).IsRequired();
            entityConfig.HasOne<StockType>()
              .WithMany()
              .HasForeignKey(x => x.StockTypeId);
        }

        public void Setup(EntityTypeBuilder<GIWDocument> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Name).IsUnique();
        }

        public void Setup(EntityTypeBuilder<GIWDocumentItem> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
        }

        public void Setup(EntityTypeBuilder<StockType> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Code).IsUnique();
        }

        public void Setup(EntityTypeBuilder<TransactionType> entityConfig)
        {
            entityConfig.HasKey(x => x.Id);
            entityConfig.HasIndex(x => x.Code).IsUnique();
        }
    }
}
