using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers
{
    public interface IGetUpgradedCustomersQuery
    {
        GetUpgradedCustomersResponse Execute(GetUpgradedCustomersRequest request);
    }
}
