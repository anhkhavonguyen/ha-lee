using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.SetPasswordForAccountCommandHandler
{
    public interface ISetPasswordForAccountCommandHandler
    {
        Task ExecuteAsync(SetPasswordForAccountCommand setPasswordForStoreAccountCommand);
    }
}
