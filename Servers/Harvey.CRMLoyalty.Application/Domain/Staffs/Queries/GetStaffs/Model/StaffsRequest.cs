using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.Staffs.Queries
{
    public class GetStaffsRequest : BaseRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string SearchString { get; set; }
    }

    public class GetStaffsByOutletRequest : BaseRequest
    {
        public string OutletId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
