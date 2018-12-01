using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ReactiveCustomerWithNewPhoneCommandHandler
{
    public class ReactiveCustomerWithNewPhoneCommand
    {
        public string UserId { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerId { get; set; }
        public int IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Data.Gender? Gender { get; set; }
        public string MemberOriginalUrl { get; set; }
        public string CreatedByName { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
