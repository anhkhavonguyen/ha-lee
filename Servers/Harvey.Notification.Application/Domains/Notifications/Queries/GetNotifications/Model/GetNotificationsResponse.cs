using Harvey.Notification.Application.Models;
using Harvey.Notification.Application.Requests;
using System.Collections.Generic;

namespace Harvey.Notification.Application.Domains.Notifications.Queries
{
    public class GetNotificationsResponse : BaseResponse
    {
        public List<NotificationModel> ListNotification { get; set; }
    }
}
