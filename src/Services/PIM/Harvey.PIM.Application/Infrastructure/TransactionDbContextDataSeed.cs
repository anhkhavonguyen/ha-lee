using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.Polly;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.PIM.Application.Infrastructure
{
    public class TransactionDbContextDataSeed
    {
        public async Task SeedAsync(TransactionDbContext context, ILogger<TransactionDbContextDataSeed> logger)
        {
            var policy = new DataSeedRetrivalPolicy();
            await policy.ExecuteStrategyAsync(logger, () =>
            {
                using (context)
                {
                    if (!context.StockTypes.Any())
                    {
                        context.StockTypes.AddRange(GetPreconfiguredStockTypes());
                    }
                    if (!context.TransactionTypes.Any())
                    {
                        context.TransactionTypes.AddRange(GetPreconfiguredTransactionTypes());
                    }
                    context.SaveChanges();
                }
            });
        }

        private List<StockType> GetPreconfiguredStockTypes()
        {
            return new List<StockType>()
            {
                new StockType()
                {
                    Code = "CON",
                    Name = "Consignment",
                    Description = "Consignment"
                },
                new StockType()
                {
                    Code = "SOR",
                    Name = "Sales or Return",
                    Description = "Sales or Return"
                },
                new StockType()
                {
                    Code = "SOE",
                    Name = "Sales or Exchange",
                    Description = "Sales or Exchange"
                },
                new StockType()
                {
                    Code = "FIRM",
                    Name = "Firm Sales",
                    Description = "Firm Sales"
                },
            };
        }

        private List<TransactionType> GetPreconfiguredTransactionTypes()
        {
            return new List<TransactionType>()
            {
                new TransactionType()
                {
                    Code = "GIW",
                    Name  = "Goods Inward",
                    Description  = "Goods Inward"
                },
                 new TransactionType()
                {
                    Code = "TFI",
                    Name  = "Transfer In",
                    Description  = "Transfer In"
                },
                  new TransactionType()
                {
                    Code = "TFO",
                    Name  = "Transfer Out",
                    Description  = "Transfer Out"
                }
            };
        }
    }
}
