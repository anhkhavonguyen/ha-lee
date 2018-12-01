using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.RedeemPointCommandHandler
{
    public interface IRedeemPointCommandHandler
    {
        Task<decimal> ExecuteAsync(RedeemPointCommand initCustomerProfileCommand);
    }
}
