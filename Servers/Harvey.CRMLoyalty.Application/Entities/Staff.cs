using System;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class Staff : EntityBase
    {
        public Staff()
        {
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
        public string PhoneCountryCode { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int Gender { get; set; }

        public string ProfileImage { get; set; }

        public TypeOfStaff TypeOfStaff { get; set; }

        public virtual ICollection<Staff_Outlet> Staff_Outlets { get; set; }
        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }
        public virtual ICollection<WalletTransaction> VoidedWalletTransactions { get; set; }
        public virtual ICollection<PointTransaction> PointTransactions { get; set; }
        public virtual ICollection<PointTransaction> VoidedPointTransactions { get; set; }
        public virtual ICollection<MembershipTransaction> MembershipTransactions { get; set; }
    }
}
