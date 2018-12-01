using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.Model
{
    public class GetWalletTransactionsRequest : BaseRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
