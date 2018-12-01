using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ExpiryPointCommandHandler
{
    public interface IExpiryPointCommandHandler
    {
        Task<decimal> ExecuteAsync(ExpiryPointCommand initCustomerProfileCommand);
    }
}
