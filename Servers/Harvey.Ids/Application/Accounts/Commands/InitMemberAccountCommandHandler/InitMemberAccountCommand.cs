namespace Harvey.Ids.Application.Accounts.Commands.InitMemberAccountCommandHandler
{
    public class InitMemberAccountCommand
    {
        public string UserId { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string OriginalUrl { get; set; }
        public string CurrentUserId { get; set; }
        public string OutletName { get; set; }
    }
}
