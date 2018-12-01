namespace Harvey.Message.Accounts
{
    public interface SendForgotPasswordEmailMessage
    {
        string Email { get; }
        string Title { get; }
        string FirstName { get; }
        string LastName { get; }
        string ShortLink { get; }
        string BrandName { get; set; }
        string AcronymBrandName { get; set; }
    }
}
