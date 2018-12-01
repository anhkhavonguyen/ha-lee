using Harvey.Ids.Domains;
using Harvey.Ids.Services.Account.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Account
{
    public interface IAccountService
    {
        Task SendEmailForgotPasswordAsync(ForgotPasswordViaEmailCommand forgotPasswordCommand, string currentPageUrl);
        Task SendMailConfirmUserAsync(ApplicationUser user, string currentPageUrl);
    }
}
