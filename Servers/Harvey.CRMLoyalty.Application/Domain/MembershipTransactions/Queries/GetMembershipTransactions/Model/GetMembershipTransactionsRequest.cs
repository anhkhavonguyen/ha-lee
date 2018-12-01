using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions.Model
{
    public class GetMembershipTransactionsRequest : BaseRequest
    {
        public string OutletId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
