using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ChangePhoneNumberCommandHandler
{
    public interface IChangePhoneNumberCommandHandler
    {
        Task ExecuteAsync(ChangePhoneNumberCommand command);
    }
}
