using System;

namespace Harvey.Notification.Application.Domains.Notifications.Queries
{
    public class GetNotificationsRequest
    {
        public string SearchText { get; set; }
        public string DateFilter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
