using Harvey.Ids.Data;
using System;

namespace Harvey.Ids.Application.Accounts.Commands.SignUpMemberCommandHandler
{
    public class SignUpMemberCommand
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public Gender? Gender { get; set; }
        public string Avatar { get; set; }
    }
}
