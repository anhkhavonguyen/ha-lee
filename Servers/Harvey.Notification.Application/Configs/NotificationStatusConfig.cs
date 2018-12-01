using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Entities;
using System.Collections.Generic;

namespace Harvey.Notification.Application.Configs
{
    public static class NotificationStatusConfig
    {
        public static List<NotificationStatus> GetNotificationStatus()
        {
            return new List<NotificationStatus>
            {
                new NotificationStatus
                {
                    Id = (int)Status.Pending,
                    DisplayName = "Pending"
                },
                new NotificationStatus
                {
                    Id = (int)Status.Fail,
                    DisplayName = "Fail"
                },
                new NotificationStatus
                {
                    Id = (int)Status.Success,
                    DisplayName = "Success"
                },
            };
        }
    }
}
