using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.ReSendSignUpLink
{
    public class ReSendSignUpLinkCommandRequest
    {
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SignUpShortLink { get; set; }
        public string OutletName { get; set; }
        public string PIN { get; set; }
    }
}
