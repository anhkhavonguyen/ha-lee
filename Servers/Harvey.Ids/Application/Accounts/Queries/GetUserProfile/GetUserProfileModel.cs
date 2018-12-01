using Harvey.Ids.Data;
using System;

namespace Harvey.Ids.Application.Accounts.Queries.GetUserProfile
{
    public class GetUserProfileModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string Avatar { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCountryCode { get; set; }
    }
}
