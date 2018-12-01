namespace Harvey.Ids.Application.Accounts.Commands.ChangePINCommandHandler
{
    public class ChangePINCommand
    {
        public string UserId { get; set; }
        public string OldPIN { get; set; }
        public string NewPIN { get; set; }
    }
}
