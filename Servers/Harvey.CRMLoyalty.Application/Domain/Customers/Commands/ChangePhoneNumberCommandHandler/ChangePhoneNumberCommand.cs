using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ChangePhoneNumberCommandHandler
{
    public class ChangePhoneNumberCommand
    {
        public string CustomerId { get; set; }
        public string UpdatedBy { get; set; }
        public string NewPhoneNumber { get; set; }
        public string NewPhoneCountryCode { get; set; }
    }
}
