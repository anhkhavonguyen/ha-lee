using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class GetVoidedCustomersResponse : BaseResponse
    {
        public List<VoidedCustomersModel> CustomerListResponse { get; set; }
    }
}
