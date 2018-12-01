using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetPremiumCustomers.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetPremiumCustomers
{
    public interface IGetPremiumCustomers
    {
        GetPremiumCustomersResponse Execute(GetPremiumCustomersRequest request);
    }
}
