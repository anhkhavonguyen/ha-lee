using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.Model;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public interface IGetWalletTransactionsQuery
    {
        GetWalletTransactionsResponse GetWalletTransactions(GetWalletTransactionsRequest request);
        GetWalletTransactionsResponse GetWalletTransactionsByStaff(GetWalletTransactionsRequest request);
        decimal GetWalletBalance(string customerId);

    }
}
