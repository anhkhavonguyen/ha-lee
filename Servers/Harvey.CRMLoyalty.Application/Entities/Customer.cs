using System;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Entities
{
    public class Customer : EntityBase
    {
        public string CustomerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? LastUsed { get; set; }
        public Status Status { get; set; }
        public string FirstOutlet { get; set; }
        public Data.Gender? Gender { get; set; }

        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }
        public virtual ICollection<PointTransaction> PointTransactions { get; set; }
        public virtual ICollection<MembershipTransaction> MembershipTransactions { get; set; }    
    }
}
