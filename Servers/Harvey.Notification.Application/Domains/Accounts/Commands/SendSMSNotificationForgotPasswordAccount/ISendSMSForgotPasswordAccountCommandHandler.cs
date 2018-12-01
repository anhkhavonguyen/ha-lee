using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendSMSNotificationForgotPasswordAccount
{
    public interface ISendSMSForgotPasswordAccountCommandHandler
    {
        Task ExecuteAsync(SendSMSForgotPasswordAccountCommand sendForgotPasswordAccountCommand);
    }
}
