using System;

namespace Harvey.CRMLoyalty.Application.Models
{
    public class CustomerModel : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string PhoneCountryCode { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? JoinedDate { get; set; }

        public DateTime? LastUsed { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        public string CustomerCode { get; set; }

        public Data.Gender? Gender { get; set; }
    }
}
