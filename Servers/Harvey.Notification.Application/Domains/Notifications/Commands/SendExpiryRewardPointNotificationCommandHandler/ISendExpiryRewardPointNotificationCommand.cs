using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler.Model;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler
{
    public interface ISendExpiryRewardPointNotificationCommand
    {
        Task Execute(SendExpiryRewardPointNotificationRequest request);
    }
}
