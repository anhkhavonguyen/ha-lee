using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ChangePINCommandHandler
{
    public interface IChangePINCommandHandler
    {
        Task ExecuteAsync(ChangePINCommand changePINCommand);
    }
}
