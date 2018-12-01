using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendEmailNotificationForgotPasswordAccount
{
    public interface ISendEmailForgotPasswordAccountCommandHandler
    {
        Task ExecuteAsync(SendEmailForgotPasswordAccountCommand sendForgotPasswordAccountCommand);
    }
}
