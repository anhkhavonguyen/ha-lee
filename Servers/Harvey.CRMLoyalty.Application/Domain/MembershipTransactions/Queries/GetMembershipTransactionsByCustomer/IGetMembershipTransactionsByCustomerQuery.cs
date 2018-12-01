using System.Collections.Generic;
using Harvey.CRMLoyalty.Application.Models;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries
{
    public interface IGetMembershipTransactionsByCustomerQuery
    {
        GetMembershipTransactionsByCustomerResponse Execute(GetMembershipTransactionsByCustomerRequest request);
    }
}
