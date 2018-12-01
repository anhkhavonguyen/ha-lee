using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ForgotPasswordViaSMSCommandHandler
{
    public interface IForgotPasswordViaSMSCommandHandler
    {
        Task ExecuteAsync(ForgotPasswordViaSMSCommand forgotPasswordCommand);
    }
}
