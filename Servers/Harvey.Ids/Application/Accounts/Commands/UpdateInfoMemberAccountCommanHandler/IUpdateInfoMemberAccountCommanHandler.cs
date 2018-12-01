using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.UpdateInfoMemberAccountCommanHandler
{
    public interface IUpdateInfoMemberAccountCommanHandler
    {
        Task ExecuteAsync(UpdateInfoMemberAccountCommand updateInfoMemberAccountCommand);
    }
}
