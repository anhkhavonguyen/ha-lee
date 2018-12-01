using System;

namespace Harvey.CRMLoyalty.Application.Models
{
    public class StaffModel : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string PhoneCountryCode { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int Gender { get; set; }

        public string ProfileImage { get; set; }

        public string FullName { get; set; }

        public string TypeOfStaff { get; set; }
    }

    public class Staff_Outlet_Model : BaseModel
    {
        public string StaffId { get; set; }
        public string OutletId { get; set; }
    }
}
