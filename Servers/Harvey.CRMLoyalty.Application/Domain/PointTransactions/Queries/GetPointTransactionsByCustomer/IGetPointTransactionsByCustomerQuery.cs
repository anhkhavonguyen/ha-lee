using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public interface IGetPointTransactionsByCustomerQuery
    {
        GetPointTransactionsByCustomerResponse Execute(GetPointTransactionsByCustomerRequest request);
        GetPointTransactionsByCustomerResponse GetByMember(GetPointTransactionsByCustomerRequest request);
    }
}
