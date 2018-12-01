using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.Model;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public interface IGetPointTransactionsQuery
    {
        GetPointTransactionsResponse GetPointTransactions(GetPointTransactionsRequest request);
        GetPointTransactionsResponse GetPointTransactionsByStaff(GetPointTransactionsRequest request);
        decimal GetPointBalance(string customerId);
        GetPointTransactionsResponse GetPointTransactionsByOutlet(GetPointTransactionsByOutletRequest request);
    }
}
