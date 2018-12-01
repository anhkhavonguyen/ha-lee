using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Services.EmailService;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendEmailNotificationForgotPasswordAccount
{
    public class SendEmailForgotPasswordAccountCommandHandler : ISendEmailForgotPasswordAccountCommandHandler
    {
        private readonly HarveyNotificationDbContext _harveyNotificationDbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public SendEmailForgotPasswordAccountCommandHandler(IEmailService emailService, 
            HarveyNotificationDbContext harveyNotificationDbContext,
            IConfiguration configuration)
        {
            _emailService = emailService;
            _harveyNotificationDbContext = harveyNotificationDbContext;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(SendEmailForgotPasswordAccountCommand sendForgotPasswordAccountCommand)
        {
            var template = _harveyNotificationDbContext.Templates.First(f => f.TemplateKey == TemplateConfig.EMAIL_RESET_PWD);
            string content = string.Format(template.Content, sendForgotPasswordAccountCommand.FirstName, sendForgotPasswordAccountCommand.LastName, sendForgotPasswordAccountCommand.ShortLink,
                                                            sendForgotPasswordAccountCommand.AcronymBrandName, sendForgotPasswordAccountCommand.BrandName);

            var notification = new Entities.Notification
            {
                NotificationTypeId = (int)NotifyType.Email,
                TemplateId = template.Id,
                Content = content,
                Receivers = sendForgotPasswordAccountCommand.Email,
                Status = (int)Status.Pending,
                Action = Entities.Action.EmailResetPassword
            };
            _harveyNotificationDbContext.Notifications.Add(notification);
            await _harveyNotificationDbContext.SaveChangesAsync();

            try
            {
                await _emailService.SendEmailAsync(_configuration["NoReplyEmail:EmailAddress"], _configuration["NoReplyEmail:DisplayName"],
                                        notification.Receivers,
                                        sendForgotPasswordAccountCommand.Title,
                                        notification.Content, true);
                notification.Status = (int)Status.Success;
                await _harveyNotificationDbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                Log.Error(ex.GetBaseException().ToString());
            }
        }
    }
}
