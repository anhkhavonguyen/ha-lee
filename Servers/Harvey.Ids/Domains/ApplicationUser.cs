using Microsoft.AspNetCore.Identity;
using System;
using Harvey.Ids.Data;

namespace Harvey.Ids.Domains
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public UserType UserType { get; set; }
        public bool IsMigrateData { get; set; }
        public string Pin { get; set; }
        public string Avatar { get; set; }
        public string ZipCode { get; set; }
        public string PhoneCountryCode { get; set; }
    }
}
