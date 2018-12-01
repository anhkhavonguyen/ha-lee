using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaEmailCommandHandler
{
    public interface IForgotPasswordViaEmailCommandHandler
    {
        Task ExecuteAsync(ForgotPasswordViaEmailCommand forgotPasswordCommand);
    }
}
