using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateFullCustomerInfomationCommandHandler
{
    public class UpdateFullCustomerInfomationCommand
    {
        public string CustomerId { get; set; }
        public string NewPhoneCountryCode { get; set; }
        public string NewPhoneNumber { get; set; }
        public string UpdatedBy { get; set; }
        public string CustomerCode { get; set; }
        public string MemberOriginalUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PostalCode { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
