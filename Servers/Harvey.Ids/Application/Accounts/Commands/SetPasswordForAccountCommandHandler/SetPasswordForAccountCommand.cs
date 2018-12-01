namespace Harvey.Ids.Application.Accounts.Commands.SetPasswordForAccountCommandHandler
{
    public class SetPasswordForAccountCommand
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
