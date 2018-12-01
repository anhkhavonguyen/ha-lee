using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetWalletTransactionsByCustomerRequest : BaseRequest
    {
        public string CustomerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
