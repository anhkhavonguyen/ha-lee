using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ResetPINCommandHandler
{
    public interface IResetPINCommandHandler
    {
        Task ExecuteAsync(ResetPINCommand resetPINCommand);
    }
}
