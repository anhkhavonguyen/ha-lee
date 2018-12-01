using System;
using System.Collections;
using System.Collections.Generic;

namespace Harvey.Message.Notifications
{
    public interface SendSmsExpiryMembershipNotificationCommand
    {
        List<ExpiryMembership> ExpiryMemberships { get; set; }
    }

    public class ExpiryMembership
    {
        public DateTime ExpiredMembershipDate { get; set; }
        public string Phone { get; set; }
        public string AcronymBrandTitle { get; set; }
        public string BrandHomeLinkUrl { get; set; }
    }
}
