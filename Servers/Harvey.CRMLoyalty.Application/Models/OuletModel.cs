using System;

namespace Harvey.CRMLoyalty.Application.Models
{
    public class OutletModel : BaseModel
    {
        public string Name { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string OutletImage { get; set; }
        public string FirstNameAccount { get; set; }
        public string LastNameAccount { get; set; }
        public string Code { get; set; }
    }
}
