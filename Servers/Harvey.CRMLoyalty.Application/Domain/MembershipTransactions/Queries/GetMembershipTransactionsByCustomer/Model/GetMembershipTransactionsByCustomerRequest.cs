using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries
{
    public class GetMembershipTransactionsByCustomerRequest : BaseRequest
    {
        public string CustomerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
