using Harvey.Ids.Data;
using System;
using System.Collections.Generic;

namespace Harvey.Ids.Application.User.Command.CreateUserProfile
{
    public class CreateUserProfile
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public bool IsActive { get; set; }
        public UserType UserType { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
