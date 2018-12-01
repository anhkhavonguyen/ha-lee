using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler.Model;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler
{
    public interface ISendExpiryMembershipNotificationCommand
    {
        Task Execute(SendExpiryMembershipNotificationRequest request);
    }
}
