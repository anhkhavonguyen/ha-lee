namespace Harvey.Message.Accounts
{
    public interface SendPINToNumberPhoneMessage
    {
        string PhoneNumber { get; }
        string PhoneCountryCode { get; }
        string Pin { get; }
        string AcronymBrandName { get; }
        string OutletName { get; }
    }
}
