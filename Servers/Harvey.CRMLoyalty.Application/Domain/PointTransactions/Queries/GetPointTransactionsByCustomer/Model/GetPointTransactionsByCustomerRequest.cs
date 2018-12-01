using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetPointTransactionsByCustomerRequest : BaseRequest
    {
        public string CustomerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
