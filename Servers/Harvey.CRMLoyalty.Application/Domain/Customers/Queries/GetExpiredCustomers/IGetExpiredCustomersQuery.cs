using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public interface IGetExpiredCustomersQuery
    {
        GetExpiredCustomersResponse Execute(GetExpiredCustomersRequest request);
    }
}
