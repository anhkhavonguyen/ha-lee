using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Customers
{
    public interface ReactiveCustomerWithNewPhoneMessageCommand
    {
        string UserId { get; set; }
        string PhoneCountryCode { get; set; }
        string PhoneNumber { get; set; }
        string CustomerId { get; set; }
        int IsActive { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        DateTime? DateOfBirth { get; set; }
        int? Gender { get; set; }
        string MemberOriginalUrl { get; set; }
        string AcronymBrandName { get; set; }
    }
}
