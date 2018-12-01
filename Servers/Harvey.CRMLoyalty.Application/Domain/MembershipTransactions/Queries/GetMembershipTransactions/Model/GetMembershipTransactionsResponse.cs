using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions.Model
{
    public class GetMembershipTransactionsResponse : BaseResponse
    {
        public List<MembershipTransactionModel> ListMembershipTransaction { get; set; }
    }
}
