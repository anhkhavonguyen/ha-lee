using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Models
{
    public class NotificationModel : BaseModel
    {
        public string Content { get; set; }
        public string Status { get; set; }
        public string NotificationType { get; set; }
        public string Action { get; set; }
        public string Receivers { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum NotificationTypeEnum
    {
        SMS = 0,
        EMAIL = 1
    }
}
