using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.FotgotPINCommandHandler
{
    public interface IFotgotPINCommandHandler
    {
        Task ExecuteAsync(FotgotPINCommand fotgotPINCommand);
    }
}
