using Amazon.SimpleNotificationService.Model;
using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Services.SMSService;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendSMSNotificationForgotPasswordAccount
{
    public class SendSMSForgotPasswordAccountCommandHandler : ISendSMSForgotPasswordAccountCommandHandler
    {
        private readonly HarveyNotificationDbContext _harveyNotificationDbContext;
        private readonly ISMSService _smsService;
        private readonly IConfiguration _configuration;

        public SendSMSForgotPasswordAccountCommandHandler(ISMSService smsService, 
            HarveyNotificationDbContext harveyNotificationDbContext,
            IConfiguration configuration)
        {
            _smsService = smsService;
            _harveyNotificationDbContext = harveyNotificationDbContext;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(SendSMSForgotPasswordAccountCommand sendForgotPasswordAccountCommand)
        {
            var template = _harveyNotificationDbContext.Templates.First(f => f.TemplateKey == TemplateConfig.SMS_RESET_PWD);
            string outletName = string.IsNullOrEmpty(sendForgotPasswordAccountCommand.OutletName) ? sendForgotPasswordAccountCommand.AcronymBrandName.ToString() : sendForgotPasswordAccountCommand.OutletName;
            string content = string.Format(template.Content, outletName, sendForgotPasswordAccountCommand.Link);

            var notification = new Entities.Notification
            {
                NotificationTypeId = (int)NotifyType.Sms,
                TemplateId = template.Id,
                Content = content,
                Receivers = $"+{sendForgotPasswordAccountCommand.PhoneCountryCode}{sendForgotPasswordAccountCommand.PhoneNumber}",
                Status = (int)Status.Pending,
                Action = Entities.Action.ForgotPassword
            };
            _harveyNotificationDbContext.Notifications.Add(notification);
            await _harveyNotificationDbContext.SaveChangesAsync();

            try
            {
                PublishResponse result = await _smsService.SendAsync(notification.Receivers, template.Title, notification.Content);
                if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    notification.Status = (int)Status.Success;
                    await _harveyNotificationDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) {
                Log.Error(ex.GetBaseException().ToString());
            }
        }
    }
}
