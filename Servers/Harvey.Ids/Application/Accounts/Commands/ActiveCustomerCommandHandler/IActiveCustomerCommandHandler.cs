using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ActiveCustomerCommandHandler
{
    public interface IActiveCustomerCommandHandler
    {
        Task ExecuteAsync(ActiveCustomerCommand command);
    }
}
