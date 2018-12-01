using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromAddress,string fromName, string toAddress, string subject, string content,bool isHtml);
        Task SendEmailAsync(MailAddress fromAddress, MailAddress toAddress, string subject, string content, bool isHtml = true);
    }
}
