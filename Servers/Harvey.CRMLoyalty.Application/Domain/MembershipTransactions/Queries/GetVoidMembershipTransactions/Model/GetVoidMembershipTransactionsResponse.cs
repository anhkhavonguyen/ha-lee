using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions.Model
{
    public class GetVoidMembershipTransactionsResponse : BaseResponse
    {
        public List<MembershipTransactionModel> ListMembershipTransaction { get; set; }
    }
}
