using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddMembershipCommandHandler
{
    public interface IAddMembershipCommandHandler
    {
        Task<string> ExecuteAsync(AddMembershipCommand initCustomerProfileCommand);
    }
}
