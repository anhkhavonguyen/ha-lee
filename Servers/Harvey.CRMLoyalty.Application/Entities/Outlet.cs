using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class Outlet : EntityBase
    {
        public Outlet()
        {
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string OutletImage { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Staff_Outlet> Staff_Outlets { get; set; }
        public virtual ICollection<WalletTransaction>  WalletTransactions { get; set; }
        public virtual ICollection<PointTransaction> PointTransactions { get; set; }
        public virtual ICollection<MembershipTransaction> MembershipTransactions { get; set; }
    }

    public enum Status
    {
        Active = 0,
        InActive = 1
    }
}
