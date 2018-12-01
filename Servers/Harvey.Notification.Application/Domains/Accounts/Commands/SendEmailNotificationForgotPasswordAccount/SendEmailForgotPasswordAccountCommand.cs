namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendEmailNotificationForgotPasswordAccount
{
    public class SendEmailForgotPasswordAccountCommand
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string ShortLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BrandName { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
