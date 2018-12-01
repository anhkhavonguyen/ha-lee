using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.ReSendSignUpLink
{
    public interface IReSendSignUpLinkCommandHandler
    {
        Task ExecuteAsync(ReSendSignUpLinkCommandRequest request);
    }
}
