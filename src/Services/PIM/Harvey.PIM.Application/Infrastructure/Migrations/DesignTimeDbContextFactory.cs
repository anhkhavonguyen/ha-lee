using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Harvey.PIM.Application.Infrastructure.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PimDbContext>
    {
        public PimDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PimDbContext>();
            var connectionString = "Server=localhost;port=5432;Database=harveypim;UserId=postgres;Password=123456";
            builder.UseNpgsql(connectionString);
            return new PimDbContext(builder.Options);
        }
    }
}
