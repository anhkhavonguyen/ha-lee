using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries
{
    public class GetMembershipTransactionsByCustomerResponse: BaseResponse
    {
        public List<MembershipTransactionModel> ListMembershipTransaction { get; set; }
    }
}
