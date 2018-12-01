using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure.Provisions
{
    public class DbProvisionTask : IProvisionTask<DbProvisionTaskOption>
    {
        private readonly ILogger<DbProvisionTask> _logger;

        public DbProvisionTask(ILogger<DbProvisionTask> logger)
        {
            _logger = logger;
        }

        public async Task<bool> ExecuteAsync(DbProvisionTaskOption options)
        {
            try
            {
                _logger.LogInformation($"Migrating database associated with context {typeof(CatalogDbContext).Name}");

                var retry = Policy.Handle<SqlException>()
                     .WaitAndRetry(new TimeSpan[]
                     {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                     });

                retry.Execute(() =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
                    optionsBuilder.UseNpgsql(options.ConnectionString);
                    using (var dbContext = new CatalogDbContext(optionsBuilder.Options))
                    {
                        dbContext.Database.Migrate();
                    };
                });

                _logger.LogInformation($"Migrated database associated with context {typeof(CatalogDbContext).Name}");
                return true;

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(CatalogDbContext).Name}");
                return false;
            }
        }

        public Task Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
