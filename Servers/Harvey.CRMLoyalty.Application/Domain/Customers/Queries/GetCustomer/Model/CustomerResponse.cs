using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetCustomer.Model
{
    public class CustomerResponse : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastUsed { get; set; }
        public string Membership { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int TotalStranstion { get; set; }
        public string CommentMembership { get; set; }
        public string CustomerCode { get; set; }

        public bool HasPreminumMembershipTransaction { get; set; }
        public int Status { get; set; }
        public Data.Gender? Gender { get; set; }
    }
}
