using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateFullCustomerInfomationCommandHandler
{
    public interface IUpdateFullCustomerInfomationCommandHandler
    {
        Task<string> ExecuteAsync(UpdateFullCustomerInfomationCommand command);
    }
}
