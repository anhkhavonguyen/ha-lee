using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendSmsChangePhoneNumber
{
    public interface ISendSmsChangePhoneNumberCommandHandler
    {
        Task ExecuteAsync(SendSmsChangePhoneNumberCommandRequest command);
    }
}
