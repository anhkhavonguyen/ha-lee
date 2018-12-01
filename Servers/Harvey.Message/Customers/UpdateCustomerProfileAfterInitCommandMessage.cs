using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Customers
{
    public interface UpdateCustomerProfileAfterInitCommandMessage
    {
        string CustomerId { get; }
        string PhoneCountryCode { get; }
        string PhoneNumber { get; set; }
        string UpdatedBy { get; }
        DateTime? DateOfBirth { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Gender { get; set; }
    }
}
