using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Harvey.PIM.Application.Infrastructure.ActivityMigrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ActivityLogDbContext>
    {
        public ActivityLogDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ActivityLogDbContext>();
            var connectionString = "Server=localhost;port=5432;Database=harveypim;UserId=postgres;Password=123456";
            builder.UseNpgsql(connectionString);
            return new ActivityLogDbContext(builder.Options);
        }
    }
}
