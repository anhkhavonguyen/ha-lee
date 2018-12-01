using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ReactiveCustomerWithNewPhoneCommandHandler
{
    public interface IReactiveCustomerWithNewPhoneCommandHandler
    {
        Task ExecuteAsync(ReactiveCustomerWithNewPhoneCommand command);
    }
}
