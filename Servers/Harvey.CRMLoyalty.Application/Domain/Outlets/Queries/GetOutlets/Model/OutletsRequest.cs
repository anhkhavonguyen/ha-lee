using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.Outlets.Queries
{
    public class GetOutletsRequest : BaseRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
