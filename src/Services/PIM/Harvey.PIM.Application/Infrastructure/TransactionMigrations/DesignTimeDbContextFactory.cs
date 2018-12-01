using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Harvey.PIM.Application.Infrastructure.TransactionMigrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TransactionDbContext>
    {
        public TransactionDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TransactionDbContext>();
            var connectionString = "Server=localhost;port=5432;Database=harveychannel;UserId=postgres;Password=123456";
            builder.UseNpgsql(connectionString);
            return new TransactionDbContext(builder.Options);
        }
    }
}
