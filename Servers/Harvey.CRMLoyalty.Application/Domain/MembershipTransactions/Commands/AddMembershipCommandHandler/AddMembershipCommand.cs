using Harvey.CRMLoyalty.Application.Requests;
using System;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddMembershipCommandHandler
{
    public class AddMembershipCommand: BaseRequest
    {
        public string CustomerId { get; set; }
        public string OutletId { get; set; }
        public string Comment { get; set; }
        public int MembershipTypeId { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public string IpAddress { get; set; }
        public string StaffId { get; set; }
        public int MembershipActionType { get; set; }
    }
}
