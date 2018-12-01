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

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendPINToNumberPhone
{
    public class SendPINToNumberPhoneCommandHandler : ISendPINToNumberPhoneCommandHandler
    {
        private readonly HarveyNotificationDbContext _harveyNotificationDbContext;
        private readonly ISMSService _smsService;
        private readonly IConfiguration _configuration;

        public SendPINToNumberPhoneCommandHandler(HarveyNotificationDbContext harveyNotificationDbContext,
            ISMSService smsService,
            IConfiguration configuration)
        {
            _harveyNotificationDbContext = harveyNotificationDbContext;
            _smsService = smsService;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(SendPINToNumberPhoneCommand sendPINToNumberPhoneCommand)
        {
            var template = _harveyNotificationDbContext.Templates.First(f => f.TemplateKey == TemplateConfig.SMS_SEND_PIN);
            string outletName = string.IsNullOrEmpty(sendPINToNumberPhoneCommand.OutletName) ? sendPINToNumberPhoneCommand.AcronymBrandName.ToString() : sendPINToNumberPhoneCommand.OutletName;
            string content = string.Format(template.Content, outletName, sendPINToNumberPhoneCommand.Pin);

            var notification = new Entities.Notification
            {
                NotificationTypeId = (int)NotifyType.Sms,
                TemplateId = template.Id,
                Content = content,
                Receivers = $"+{sendPINToNumberPhoneCommand.PhoneCountryCode}{sendPINToNumberPhoneCommand.PhoneNumber}",
                Status = (int)Status.Pending,
                Action = Entities.Action.ReSendPin
            };
            _harveyNotificationDbContext.Notifications.Add(notification);
            await _harveyNotificationDbContext.SaveChangesAsync();

            try
            {
                var templateTitle = string.Format(template.Title, sendPINToNumberPhoneCommand.AcronymBrandName);
                PublishResponse result = await _smsService.SendAsync(notification.Receivers, templateTitle, notification.Content);
                if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    notification.Status = (int)Status.Success;
                    await _harveyNotificationDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetBaseException().ToString());
            }
        }
    }
}
