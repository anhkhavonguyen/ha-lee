using Harvey.CRMLoyalty.Api;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionBalance
{
    internal class GetWalletTransactionBalance : IGetWalletTransactionBalance
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetWalletTransactionBalance(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<decimal> ExecuteAsync(string customerId)
        {
            var customerLastedTransaction = await _dbContext.WalletTransactions.AsNoTracking().Where(a => a.CustomerId == customerId).OrderByDescending(a => a.CreatedDate).FirstOrDefaultAsync();
            return customerLastedTransaction != null ? customerLastedTransaction.BalanceTotal : 0;
        }
    }
}
