using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions
{
    public interface IGetMembershipTransactions
    {
        GetMembershipTransactionsResponse Execute(GetMembershipTransactionsRequest request);
    }
}
