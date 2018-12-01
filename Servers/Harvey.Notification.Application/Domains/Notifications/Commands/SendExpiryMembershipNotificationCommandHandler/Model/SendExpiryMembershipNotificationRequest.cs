using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler.Model
{
    public class SendExpiryMembershipNotificationRequest
    {
        public List<SendExpiryMembershipModel> SendExpiryMemberships { get; set; }
    }
}
