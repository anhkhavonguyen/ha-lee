using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateCustomerCodeCommandHandler
{
    public interface IUpdateCustomerCodeCommandHandler
    {
        Task ExcuteAsync(UpdateCustomerCodeCommand models);
    }
}
