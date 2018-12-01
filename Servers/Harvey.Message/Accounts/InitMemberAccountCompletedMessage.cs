namespace Harvey.Message.Accounts
{
    public interface InitMemberAccountCompletedMessage
    {
        string PhoneCountryCode { get; }
        string PhoneNumber { get; }
        string SignUpShortLink { get; set; }
        string Pin { get; }
        string CreatedBy { get; }
        string OutletName { get; }
    }
}
