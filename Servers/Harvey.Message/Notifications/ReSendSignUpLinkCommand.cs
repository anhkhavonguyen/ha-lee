using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Notifications
{
    public interface ReSendSignUpLinkCommand
    {
        string CountryCode { get; }
        string PhoneNumber { get; }
        string SignUpShortLink { get; }
        string OutletName { get;  }
        string PIN { get;  }
    }
}
