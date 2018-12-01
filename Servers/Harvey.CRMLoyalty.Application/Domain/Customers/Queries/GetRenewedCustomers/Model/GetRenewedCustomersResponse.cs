using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetRenewedCustomers.Model
{
    public class GetRenewedCustomersResponse : BaseResponse
    {
        public List<RenewedCustomersModel> CustomerListResponse { get; set; }
    }
}
