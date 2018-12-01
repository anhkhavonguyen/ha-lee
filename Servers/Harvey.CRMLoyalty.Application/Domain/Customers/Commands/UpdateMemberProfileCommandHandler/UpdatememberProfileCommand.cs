using System;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateMemberProfileCommandHandler
{
    public class UpdatememberProfileCommand
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
    }
}
