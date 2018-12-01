using System;
using System.Collections.Generic;

namespace Harvey.Message.Notifications
{
    public interface SendSmsExpiryRewardPointNotificationCommand
    {
        List<CustomerIncludeExpiryRewardPoint> CustomersIncludeExpiryRewardPoint { get; set; }
    }

    public class CustomerIncludeExpiryRewardPoint
    {
        public string Phone { get; set; }
        public string AcronymBrandTitle { get; set; }
        public string BrandHomeLinkUrl { get; set; }
        public decimal ExpiringPoints { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
