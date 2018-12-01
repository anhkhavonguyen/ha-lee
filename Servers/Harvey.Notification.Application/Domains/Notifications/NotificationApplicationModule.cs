using Harvey.Notification.Application.Domains.Notifications.Commands.SendAllSMSNotificationCommandHandler;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler;
using Harvey.Notification.Application.Domains.Notifications.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Notification.Application.Domains.Notifications
{
    public class NotificationApplicationModule
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<IGetNotificationsQuery, GetNotificationsQuery>();
            services.AddScoped<ISendAllSMSNotificationCommandHanlder, SendAllSMSNotificationCommandHanlder>();
            services.AddScoped<ISendExpiryMembershipNotificationCommand, SendExpiryMembershipNotificationCommand>();
            services.AddScoped<ISendExpiryRewardPointNotificationCommand, SendExpiryRewardPointNotificationCommand>();
        }
    }
}
