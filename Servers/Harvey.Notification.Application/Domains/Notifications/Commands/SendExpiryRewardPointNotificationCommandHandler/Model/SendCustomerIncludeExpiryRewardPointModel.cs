using System;
using System.Collections.Generic;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler.Model
{
    public class SendCustomerIncludeExpiryRewardPointModel
    {
        public string Phone { get; set; }
        public string AcronymBrandTitle { get; set; }
        public string BrandHomeLinkUrl { get; set; }
        public decimal ExpiringPoints { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
