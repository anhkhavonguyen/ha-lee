using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Message.Notifications
{
    public interface SendSmsChangePhoneNumberCommand
    {
        string NewPhoneCountryCode { get; }
        string NewPhoneNumber { get; }
        string ResetPasswordShortLink { get; }
        string UpdateProfileShortLink { get; }
        string LoginShortLink { get; }
        string UpdatedBy { get; }
        string AcronymBrandName { get; }
    }
}
