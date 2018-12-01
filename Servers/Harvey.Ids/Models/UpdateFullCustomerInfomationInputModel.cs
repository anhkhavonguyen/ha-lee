using System;
using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Models
{
    public class UpdateFullCustomerInfomationInputModel
    {
        public string CustomerId { get; set; }
        public string NewPhoneCountryCode { get; set; }
        public string NewPhoneNumber { get; set; }
        public string CustomerCode { get; set; }
        public string MemberOriginalUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Data.Gender Gender { get; set; }
        public string PostalCode { get; set; }
    }
}
