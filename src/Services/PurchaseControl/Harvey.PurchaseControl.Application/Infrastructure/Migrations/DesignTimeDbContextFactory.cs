using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Harvey.PurchaseControl.Application.Infrastructure.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PurchaseControlDbContext>
    {
        public PurchaseControlDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PurchaseControlDbContext>();
            var connectionString = "Server=localhost;port=5432;Database=harveypurchaseorder;UserId=postgres;Password=123456";
            builder.UseNpgsql(connectionString);
            return new PurchaseControlDbContext(builder.Options);
        }
    }
}
