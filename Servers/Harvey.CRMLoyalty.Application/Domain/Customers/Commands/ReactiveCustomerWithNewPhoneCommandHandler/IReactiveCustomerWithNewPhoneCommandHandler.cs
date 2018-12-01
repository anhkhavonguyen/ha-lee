using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ReactiveCustomerWithNewPhoneCommandHandler
{
    public interface IReactiveCustomerWithNewPhoneCommandHandler
    {
        Task<bool> ExecuteAsync(ReactiveCustomerWithNewPhoneCommand command);
    }
}
