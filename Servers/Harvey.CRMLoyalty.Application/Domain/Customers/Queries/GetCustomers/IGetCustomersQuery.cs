using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public interface IGetCustomersQuery
    {
        CustomersResponse Execute(CustomersRequest request);

        CustomersResponse GetCustomersbyCustomerCodes(CustomersRequest request);
    }
}
