using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetCurrentMembership.Model
{
    public class GetMembershipTransactionResponse
    {
        public string Membership { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
