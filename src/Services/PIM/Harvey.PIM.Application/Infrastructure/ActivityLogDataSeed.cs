using Harvey.Polly;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure
{
    public class ActivityLogDataSeed
    {
        public Task SeedAsync(ActivityLogDbContext context, ILogger<ActivityLogDataSeed> logger)
        {
            return Task.CompletedTask;
            //var policy = new DataSeedRetrivalPolicy();
            //await policy.ExecuteStrategyAsync(logger, () =>
            //{
            //});
        }
    }
}
