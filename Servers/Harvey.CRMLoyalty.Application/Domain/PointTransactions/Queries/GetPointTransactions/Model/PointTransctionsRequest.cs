using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.Model
{
    public class GetPointTransactionsRequest : BaseRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetPointTransactionsByOutletRequest : BaseRequest
    {
        public string OutletId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
