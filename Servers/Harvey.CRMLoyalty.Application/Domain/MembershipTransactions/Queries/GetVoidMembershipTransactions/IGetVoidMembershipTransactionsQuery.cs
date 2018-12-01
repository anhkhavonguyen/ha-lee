using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions
{
    public interface IGetVoidMembershipTransactionsQuery
    {
        GetVoidMembershipTransactionsResponse Execute(GetVoidMembershipTransactionsRequest request);
    }
}
