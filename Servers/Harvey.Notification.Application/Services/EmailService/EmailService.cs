using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Services.EmailService
{
    internal class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string fromAddress, string fromName, string toAddress, string subject, string content,  bool isHtml)
        {
            if (Boolean.Parse(_configuration["Testing:IsTesting"]) == true)
            {
                toAddress = _configuration["Testing:EmailReceivers"];
            }

            MailAddress fromEmail = new MailAddress(fromAddress, fromName);
            var emailList = toAddress.Split(";").ToList();
            var emails = new List<MailAddress>();

            emailList.ForEach(email =>
            {
                emails.Add(new MailAddress(email));
            });

            await SendEmailAsync(fromEmail, emails, subject, content, isHtml);
        }

        public async Task SendEmailAsync(MailAddress fromAddress, MailAddress toAddress, string subject, string content, bool isHtml = true)
        {
            if (Boolean.Parse(_configuration["Testing:IsTesting"]) == true)
            {
                var _emails = _configuration["Testing:EmailReceivers"];
                var emailList = _emails.Split(";").ToList();
                toAddress = new MailAddress(emailList[0]);
            }

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = _configuration["EmailService:SMTPAddress"];
            client.Credentials = new NetworkCredential(_configuration["EmailService:SMTPUsername"], _configuration["EmailService:SMTPPassword"]);
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = isHtml;

            await Task.Factory.StartNew(() => {
                client.Send(mailMessage);
            });
        }

        private async Task SendEmailAsync(MailAddress fromAddress, List<MailAddress> toAddress, string subject, string content, bool isHtml = true)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = _configuration["EmailService:SMTPAddress"];
            client.Credentials = new NetworkCredential(_configuration["EmailService:SMTPUsername"], _configuration["EmailService:SMTPPassword"]);
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = fromAddress;
            toAddress.ForEach(toEmail =>
            {
                mailMessage.To.Add(toEmail);
            });
            mailMessage.Subject = subject;
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = isHtml;

            await Task.Factory.StartNew(() => {
                client.Send(mailMessage);
            });
        }

    }
}
