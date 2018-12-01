namespace Harvey.Message.Customers
{
    public interface InitMemberProfileCommand
    {
        string PhoneNumber { get; }
        string UserId { get; }
        string PhoneCountryCode { get; }
        string CreatedBy { get; }
        string OriginalUrl { get; }
        string OutletName { get; }
    }
}
