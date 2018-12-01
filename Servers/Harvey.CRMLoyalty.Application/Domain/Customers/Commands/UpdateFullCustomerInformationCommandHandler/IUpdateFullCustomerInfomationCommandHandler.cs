using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.UpdateFullCustomerInfomationCommandHandler
{
    public interface IUpdateFullCustomerInfomationCommandHandler
    {
        Task ExecuteAsync(UpdateFullCustomerInfomationCommand command);
    }
}
