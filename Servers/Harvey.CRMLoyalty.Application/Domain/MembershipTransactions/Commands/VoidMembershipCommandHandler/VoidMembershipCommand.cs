using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.VoidMembershipCommandHandler
{
    public class VoidMembershipCommand : BaseRequest
    {
        public string MembershipTransactionId { get; set; }
        public string IpAddress { get; set; }
        public string VoidByName { get; set; }
        public string OutletId { get; set; }
        public int MembershipActionType { get; set; }
    }
}
