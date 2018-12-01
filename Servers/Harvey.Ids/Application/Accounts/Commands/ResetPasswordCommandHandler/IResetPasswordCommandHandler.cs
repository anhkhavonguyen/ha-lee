using Harvey.Ids.Domains;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ResetPasswordCommandHandler
{
    public interface IResetPasswordCommandHandler
    {
        Task ExecuteAsync(string code, string password);
    }
}
