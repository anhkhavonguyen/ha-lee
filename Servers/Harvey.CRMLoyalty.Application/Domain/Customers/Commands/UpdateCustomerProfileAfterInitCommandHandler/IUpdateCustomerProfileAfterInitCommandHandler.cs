using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerProfileAfterInitCommandHandler
{
    public interface IUpdateCustomerProfileAfterInitCommandHandler
    {
        Task ExecuteAsync(UpdateCustomerProfileAfterInitCommand command);
    }
}
