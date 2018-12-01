using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Harvey.PIM.Application.Infrastructure.CatalogMigrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CatalogDbContext>();
            var connectionString = "Server=localhost;port=5432;Database=harveychannel;UserId=postgres;Password=123456";
            builder.UseNpgsql(connectionString);
            return new CatalogDbContext(builder.Options);
        }
    }
}
