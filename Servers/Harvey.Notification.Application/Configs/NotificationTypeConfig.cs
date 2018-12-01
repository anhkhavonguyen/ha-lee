using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Entities;
using System.Collections.Generic;

namespace Harvey.Notification.Application.Configs
{
    public static class NotificationTypeConfig
    {
        public static List<NotificationType> GetNotificationTypes()
        {
            return new List<NotificationType>
            {
                new NotificationType{
                    Id = (int)NotifyType.Sms,
                    TypeName ="SMS"
                },
                new NotificationType{
                    Id = (int)NotifyType.Email,
                    TypeName ="EMAIL"
                },
            };
        }
    }
}
