using Harvey.Ids.Data;
using System;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateInfoMemberAccountCommanHandler
{
    public class UpdateInfoMemberAccountCommand
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
        public Gender? Gender { get; set; }
        public string Avatar { get; set; }
        public string CurrentUserId { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
