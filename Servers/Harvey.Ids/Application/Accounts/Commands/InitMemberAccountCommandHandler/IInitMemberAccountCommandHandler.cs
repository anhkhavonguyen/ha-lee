using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.InitMemberAccountCommandHandler
{
    public interface IInitMemberAccountCommandHandler
    {
        Task ExecuteAsync(InitMemberAccountCommand initMemberAccountCommand);
    }
}
