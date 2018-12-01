using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Harvey.Ids.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ISendGridClient _client;
        private readonly EmailSenderOptions _options;
        private readonly IConfiguration _configuration;

        public EmailSender(ISendGridClient client, 
            IOptions<EmailSenderOptions> optionsAccessor,
            IConfiguration configuration
            )
        {
            _client = client;
            _options = optionsAccessor.Value;
            _configuration = configuration;
        }

        public Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            if (emails == null && !emails.Any())
            {
                throw new Exception("Emails are missing.");
            }
            
            var myMessage = new SendGridMessage();
            var listEmailAddress = new List<EmailAddress>();
            foreach (var item in emails)
            {
                listEmailAddress.Add(new EmailAddress(item));
            }
            myMessage.AddTos(listEmailAddress);
            myMessage.From = new EmailAddress(_options.FromAddress, _options.FromName);
            myMessage.Subject = subject;
            myMessage.PlainTextContent = message;
            myMessage.HtmlContent = message;

            return _client.SendEmailAsync(myMessage);
        }
    }
}
