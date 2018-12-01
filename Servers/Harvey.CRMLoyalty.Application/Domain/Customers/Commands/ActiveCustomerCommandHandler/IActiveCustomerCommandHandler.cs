using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ActiveCustomerCommandHandler
{
    public interface IActiveCustomerCommandHandler
    {
        Task<bool> ExecuteAsync(ActiveCustomerCommand command);
    }
}
