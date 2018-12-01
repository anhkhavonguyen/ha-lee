using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler.Model
{
    public class SendExpiryMembershipModel
    {
        public DateTime ExpiryMembershipDate { get; set; }
        public string Phone { get; set; }
        public string AcronymBrandTitle { get; set; }
        public string BrandHomeLinkUrl { get; set; }
    }
}
