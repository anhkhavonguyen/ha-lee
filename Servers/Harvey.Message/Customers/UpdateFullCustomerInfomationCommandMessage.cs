using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Customers
{
    public interface UpdateFullCustomerInfomationCommandMessage
    {
        string CustomerId { get; }
        string NewPhoneCountryCode { get; }
        string NewPhoneNumber { get; set; }
        string UpdatedBy { get; }

        DateTime? DateOfBirth { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Gender { get; set; }
        string PostalCode { get; set; }
        string AcronymBrandName { get; set; }
    }
}
