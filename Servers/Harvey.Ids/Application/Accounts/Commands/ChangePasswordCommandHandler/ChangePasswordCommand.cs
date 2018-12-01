using System.ComponentModel.DataAnnotations;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePasswordCommandHandler
{
    public class ChangePasswordCommand
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
