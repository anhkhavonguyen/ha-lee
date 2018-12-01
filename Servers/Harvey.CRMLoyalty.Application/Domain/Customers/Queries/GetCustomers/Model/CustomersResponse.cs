using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class CustomersResponse : BaseResponse
    {
        public List<CustomerModel> CustomerListResponse { get; set; }
    }
}
