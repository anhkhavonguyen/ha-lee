using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetRenewedCustomers.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetRenewedCustomers
{
    public interface IGetRenewedCustomersQuery
    {
        GetRenewedCustomersResponse Execute(GetRenewedCustomersRequest request);
    }
}
