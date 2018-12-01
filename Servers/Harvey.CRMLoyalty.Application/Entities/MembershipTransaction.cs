using System;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class MembershipTransaction : EntityBase
    {
        public string StaffId { get; set; }
        public string CustomerId { get; set; }
        public string OutletId { get; set; }
        public int MembershipTypeId { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Comment { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Outlet Outlet { get; set; }
        public virtual MembershipType MembershipType { get; set; }
        public string MembershipTransactionReferenceId { get; set; }
        public virtual MembershipTransaction MembershipTransactionReference { get; set; }
        public int MembershipActionTypeId { get; set; }
        public virtual MembershipActionType MembershipActionTypeRef { get; set; }
    }
}
