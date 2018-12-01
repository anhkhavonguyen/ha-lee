using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerProfileAfterInitCommandHandler
{
    public class UpdateCustomerProfileAfterInitCommand
    {
        public string CustomerId { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string UpdatedBy { get; set; }
        public string CustomerCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}
