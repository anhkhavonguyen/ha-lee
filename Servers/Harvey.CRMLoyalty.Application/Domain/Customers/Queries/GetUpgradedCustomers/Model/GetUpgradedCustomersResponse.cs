using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers.Model
{
    public class GetUpgradedCustomersResponse : BaseResponse
    {
        public List<UpgradedCustomerModel> CustomerListResponse { get; set; }
    }
}
