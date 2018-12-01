using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Customers
{
    public interface ChangePhoneNumberCommandMessage
    {
        string CustomerId { get; }
        string UpdatedBy { get; }
        string NewPhoneNumber { get; }
        string NewPhoneCountryCode { get; }
        string AcronymBrandName { get; }
    }
}
