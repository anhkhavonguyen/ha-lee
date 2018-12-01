using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.InitCustomerProfileCommandHandler
{
    public interface IInitCustomerProfileCommandHandler
    {
        Task<string> ExecuteAsync(InitCustomerProfileCommand command);
    }
}
