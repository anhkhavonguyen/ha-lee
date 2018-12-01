using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetPremiumCustomers.Model
{
    public class GetPremiumCustomersResponse : BaseResponse
    {
        public List<PremiumCustomersModel> CustomerListResponse { get; set; }
    }
}
