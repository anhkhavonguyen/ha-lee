using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateCustomerProfileCommandHandler
{
    public interface IUpdateCustomerProfileCommandHandler
    {
        Task ExecuteAsync(UpdateCustomerProfileCommand updateCustomerProfileCommand);
    }
}
