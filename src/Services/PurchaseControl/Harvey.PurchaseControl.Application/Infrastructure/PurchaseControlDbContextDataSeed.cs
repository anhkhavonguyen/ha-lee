using Harvey.Polly;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PurchaseControl.Application.Infrastructure
{
    public class PurchaseControlDbContextDataSeed
    {
        public async Task SeedAsync(PurchaseControlDbContext context, ILogger<PurchaseControlDbContext> logger)
        {
            var policy = new DataSeedRetrivalPolicy();
            await policy.ExecuteStrategyAsync(logger, () =>
             {
                 using (context)
                 {
                     context.SaveChanges();
                 }
             });
        }
    }
}
