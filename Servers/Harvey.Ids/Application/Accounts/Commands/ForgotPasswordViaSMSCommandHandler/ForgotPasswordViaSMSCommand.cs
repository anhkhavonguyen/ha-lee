using Harvey.Ids.Domains;

namespace Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaSMSCommandHandler
{
    public class ForgotPasswordViaSMSCommand
    {
        public ApplicationUser User { get; set; }
        public string OriginalUrl { get; set; }
        public string AcronymBrandName { get; set; }
        public string OutletName { get; set; }
    }
}
