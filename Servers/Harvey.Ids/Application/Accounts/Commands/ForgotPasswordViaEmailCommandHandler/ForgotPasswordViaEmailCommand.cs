using Harvey.Ids.Domains;

namespace Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaEmailCommandHandler
{
    public class ForgotPasswordViaEmailCommand
    {
        public ApplicationUser User { get; set; }
        public string OriginalUrl { get; set; }
        public string AcronymBrandName { get; set; }
        public string BrandName { get; set; }
    }
}
