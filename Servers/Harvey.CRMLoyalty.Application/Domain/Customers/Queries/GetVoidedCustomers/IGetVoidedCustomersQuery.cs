using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public interface IGetVoidedCustomersQuery
    {
        GetVoidedCustomersResponse Execute(GetVoidedCustomersRequest request);
    }
}
