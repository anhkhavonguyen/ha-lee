using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class GetNewCustomersResponse : BaseResponse
    {
        public List<CustomerModel> CustomerListResponse { get; set; }
    }
}
