using Harvey.CRMLoyalty.Application.Requests;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer.Model
{
    public class CustomerRequest : BaseRequest
    {
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string CustomerId { get; set; }
        public bool IsServing { get; set; }
        public string StaffId { get; set; }
        public string OutletId { get; set; }
    }
}
