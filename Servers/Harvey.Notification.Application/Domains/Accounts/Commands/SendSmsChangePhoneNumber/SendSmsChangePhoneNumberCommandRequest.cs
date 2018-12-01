using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendSmsChangePhoneNumber
{
    public class SendSmsChangePhoneNumberCommandRequest
    {
        public string NewPhoneCountryCode { get; set; }
        public string NewPhoneNumber { get; set; }
        public string ResetPasswordShortLink { get; set; }
        public string UpdateProfileShortLink { get; set; }
        public string LoginShortLink { get; set; }
        public string UpdatedBy { get; set; }
        public string AcronymBrandName { get; set; }
    }
}
