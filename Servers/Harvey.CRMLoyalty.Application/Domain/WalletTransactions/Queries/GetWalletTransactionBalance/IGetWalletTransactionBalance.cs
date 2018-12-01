using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionBalance
{
    public interface IGetWalletTransactionBalance
    {
        Task<decimal> ExecuteAsync(string customerId);
    }
}
