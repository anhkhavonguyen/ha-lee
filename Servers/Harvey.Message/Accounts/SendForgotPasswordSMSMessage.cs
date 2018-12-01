namespace Harvey.Message.Accounts
{
    public interface SendForgotPasswordSMSMessage
    {
        string PhoneNumber { get; }
        string PhoneCountryCode { get; }
        string Title { get; }
        string Link { get; }
        string AcronymBrandName { get; }
        string OutletName { get; }
    }
}
