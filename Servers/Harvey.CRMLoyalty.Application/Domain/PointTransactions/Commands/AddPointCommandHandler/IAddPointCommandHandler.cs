using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddPointCommandHandler
{
    public interface IAddPointCommandHandler
    {
        Task<decimal> ExecuteAsync(AddPointCommand initCustomerProfileCommand);
    }
}
