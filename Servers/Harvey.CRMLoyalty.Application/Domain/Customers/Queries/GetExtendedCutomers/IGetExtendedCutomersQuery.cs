using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetExtendedCutomers.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetExtendedCutomers
{
    public interface IGetExtendedCutomersQuery
    {
        GetExtendedCutomersResponse Execute(GetExtendedCutomersRequest request);
    }
}
