using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ReSendSignUpLinkCommandHandler
{
    public interface IReSendSignUpLinkCommandHandler
    {
        Task ExecuteAsync(ReSendSignUpLinkCommandRequest command);
    }
}
