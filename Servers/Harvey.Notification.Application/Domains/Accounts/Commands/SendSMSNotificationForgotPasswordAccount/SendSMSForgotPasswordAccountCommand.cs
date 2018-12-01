namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendSMSNotificationForgotPasswordAccount
{
    public class SendSMSForgotPasswordAccountCommand
    {
        public string PhoneNumber { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string AcronymBrandName { get; set; }
        public string OutletName { get; set; }
    }
}
