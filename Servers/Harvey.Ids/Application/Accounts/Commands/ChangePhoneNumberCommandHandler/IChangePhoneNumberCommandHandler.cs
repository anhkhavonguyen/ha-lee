using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePhoneNumberCommandHandler
{
    public interface IChangePhoneNumberCommandHandler
    {
        Task<string> ExecuteAsync(ChangePhoneNumberCommand command);
    }
}
