using System;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateFullCustomerInfomationCommandHandler
{
    public class UpdateFullCustomerInfomationCommand
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string NewPhoneCountryCode { get; set; }
        public string NewPhoneNumber { get; set; }
        public string CustomerCode { get; set; }
        public string MemberOriginalUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Data.Gender? Gender { get; set; }
        public string PostalCode { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
