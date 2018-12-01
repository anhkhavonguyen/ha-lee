using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Application.Accounts.Commands.ReSendSignUpLinkCommandHandler
{
    public class ReSendSignUpLinkCommandRequest
    {
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string OriginalUrl { get; set; }
        public string UserId { get; set; }
        public string OutletName { get; set; }
    }
}
