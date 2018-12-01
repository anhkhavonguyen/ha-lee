using System.Collections.Generic;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler.Model
{
    public class SendExpiryRewardPointNotificationRequest
    {
        public List<SendCustomerIncludeExpiryRewardPointModel> SendCustomersIncludeExpiryRewardPointModel { get; set; }
    }
}
