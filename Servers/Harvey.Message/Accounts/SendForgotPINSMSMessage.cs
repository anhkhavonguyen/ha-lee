namespace Harvey.Message.Accounts
{
    public interface SendForgotPINSMSMessage
    {
        string PhoneNumber { get; }
        string Title { get; }
        string Content { get; set; }
    }
}
