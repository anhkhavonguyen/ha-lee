using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Harvey.Ids.Services;

namespace Harvey.Ids.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            var emails = new List<string>()
            {
                email
            };
            return emailSender.SendEmailAsync(emails, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
