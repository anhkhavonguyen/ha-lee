namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendPINToNumberPhone
{
    public class SendPINToNumberPhoneCommand
    {
        public string PhoneNumber { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Pin { get; set; }
        public string AcronymBrandName { get; set; }
        public string OutletName { get; set; }
    }
}
