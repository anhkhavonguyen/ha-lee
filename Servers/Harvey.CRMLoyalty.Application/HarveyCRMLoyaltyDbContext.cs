using Harvey.CRMLoyalty.Application.Entities;
using Microsoft.EntityFrameworkCore;


namespace Harvey.CRMLoyalty.Api
{
    public class HarveyCRMLoyaltyDbContext : DbContext
    {
        public HarveyCRMLoyaltyDbContext(DbContextOptions<HarveyCRMLoyaltyDbContext> options) : base(options)
        {

        }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Staff_Outlet> Staff_Outlets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<ErrorLogEntry> ErrorLogEntries { get; set; }
        public DbSet<ErrorLogSource> ErrorLogSources { get; set; }
        public DbSet<MembershipTransaction> MembershipTransactions { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
        public DbSet<PointTransactionType> PointTransactionType { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<AppSettingType> AppSettingTypes { get; set; }
        public DbSet<MembershipActionType> MembershipActionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MembershipType>()
               .HasKey(x => x.Id);

            modelBuilder.Entity<MembershipType>()
                .HasMany(s => s.MembershipTransactions)
                .WithOne(a => a.MembershipType)
                .HasForeignKey(b => b.MembershipTypeId);

            modelBuilder.Entity<MembershipActionType>()
             .HasKey(x => x.Id);

            modelBuilder.Entity<MembershipActionType>()
               .Property(a => a.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<MembershipActionType>().HasData(
                new MembershipActionType { Id = 0, TypeName = "Migration" },
                new MembershipActionType { Id = 1, TypeName = "New" },
                new MembershipActionType { Id = 2, TypeName = "Upgraded" },
                new MembershipActionType { Id = 3, TypeName = "Renew" },
                new MembershipActionType { Id = 4, TypeName = "Extend" },
                new MembershipActionType { Id = 5, TypeName = "Downgrade" },
                new MembershipActionType { Id = 6, TypeName = "Void" },
                new MembershipActionType { Id = 7, TypeName = "ChangeExpiry" },
                new MembershipActionType { Id = 8, TypeName = "Comment" });
            modelBuilder.Entity<Outlet>()
               .HasKey(x => x.Id);

            modelBuilder.Entity<Staff>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Staff_Outlet>()
                .HasKey(x => new { x.StaffId, x.OutletId });

            modelBuilder.Entity<Customer>()
                .HasMany(s => s.MembershipTransactions)
                .WithOne(g => g.Customer)
                .HasForeignKey(s => s.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(s => s.WalletTransactions)
                .WithOne(g => g.Customer)
                .HasForeignKey(s => s.CustomerId);

            modelBuilder.Entity<Customer>()
               .HasMany(s => s.PointTransactions)
               .WithOne(g => g.Customer)
               .HasForeignKey(s => s.CustomerId);

            modelBuilder.Entity<Staff>()
                .HasMany(s => s.MembershipTransactions)
                .WithOne(g => g.Staff)
                .HasForeignKey(s => s.StaffId);

            modelBuilder.Entity<Staff>()
                .HasMany(s => s.WalletTransactions)
                .WithOne(g => g.Staff)
                .HasForeignKey(s => s.StaffId);

            modelBuilder.Entity<Staff>()
                .HasMany(s => s.WalletTransactions)
                .WithOne(g => g.Staff)
                .HasForeignKey(s => s.VoidedById);

            modelBuilder.Entity<Staff>()
              .HasMany(s => s.PointTransactions)
              .WithOne(g => g.Staff)
              .HasForeignKey(s => s.VoidedById);

            modelBuilder.Entity<Outlet>()
               .HasMany(s => s.MembershipTransactions)
               .WithOne(g => g.Outlet)
               .HasForeignKey(s => s.OutletId);

            modelBuilder.Entity<Outlet>()
                .HasMany(s => s.WalletTransactions)
                .WithOne(g => g.Outlet)
                .HasForeignKey(s => s.OutletId);

            modelBuilder.Entity<Outlet>()
               .HasMany(s => s.PointTransactions)
               .WithOne(g => g.Outlet)
               .HasForeignKey(s => s.OutletId);

            modelBuilder.Entity<PointTransaction>(entity =>
            {
                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.PointTransactions)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PointTransaction_Staff");

                entity.HasOne(d => d.VoidedBy)
                   .WithMany(p => p.VoidedPointTransactions)
                   .HasForeignKey(d => d.VoidedById)
                   .OnDelete(DeleteBehavior.SetNull)
                   .HasConstraintName("FK_VoidedPointTransaction_Staff");

                entity.HasOne(c => c.PointTransactionReference)
                  .WithMany(d => d.VoidPointTransactions)
                  .HasForeignKey(c => c.PointTransactionReferenceId);


                entity.HasOne(d => d.PointTransactionType)
                    .WithMany(p => p.PointTransactions)
                    .HasForeignKey(d => d.PointTransactionTypeId);
            });

            modelBuilder.Entity<WalletTransaction>(entity =>
            {
                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.WalletTransactions)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_WalletTransaction_Staff");

                entity.HasOne(d => d.VoidedBy)
                   .WithMany(p => p.VoidedWalletTransactions)
                   .HasForeignKey(d => d.VoidedById)
                   .OnDelete(DeleteBehavior.SetNull)
                   .HasConstraintName("FK_VoidedWalletTransaction_Staff");

                entity.HasOne(c => c.WalletTransactionReference)
                  .WithOne();

            });

            modelBuilder.Entity<MembershipTransaction>(entity =>
            {
                entity.HasOne(c => c.MembershipTransactionReference)
                  .WithOne();

                entity.HasOne(c => c.MembershipActionTypeRef)
                    .WithMany(d => d.MembershipTransactionRefs)
                    .HasForeignKey(c => c.MembershipActionTypeId);
            });

            modelBuilder.Entity<PointTransactionType>()
                .HasKey(a => a.Id);


            modelBuilder.Entity<PointTransactionType>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<AppSettingType>()
           .HasKey(a => a.Id);

            modelBuilder.Entity<AppSettingType>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<AppSetting>()
               .HasKey(a => a.Id);

            modelBuilder.Entity<AppSetting>()
                    .HasOne(d => d.AppSettingType)
                    .WithMany(a => a.AppSettings)
                    .HasForeignKey(a => a.AppSettingTypeId);

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
