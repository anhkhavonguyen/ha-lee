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

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendNotificationInitAccount
{
    public class SendNotificationInitAccountCommandHandler : ISendNotificationInitAccountCommandHandler
    {
        private readonly HarveyNotificationDbContext _harveyNotificationDbContext;
        private readonly ISMSService _smsService;

        public SendNotificationInitAccountCommandHandler(HarveyNotificationDbContext harveyNotificationDbContext, ISMSService smsService)
        {
            _harveyNotificationDbContext = harveyNotificationDbContext;
            _smsService = smsService;
        }

        public async Task ExecuteAsync(string countryCode , string phoneNumber,string outletName, string signUpShortLink, string pin)
        {
            string phonenumber = $"+{countryCode}{phoneNumber}";
            var template = _harveyNotificationDbContext.Templates.First(f => f.TemplateKey == TemplateConfig.SMS_INIT_ACCOUNT);
            string content = string.Format(template.Content, outletName, pin, signUpShortLink);

            var notification = new Entities.Notification
            {
                NotificationTypeId = (int)NotifyType.Sms,
                TemplateId = template.Id,
                Content = content,
                Receivers = phonenumber,
                Status = (int)Status.Pending,
                Action = Entities.Action.Welcome
            };
            _harveyNotificationDbContext.Notifications.Add(notification);
            await _harveyNotificationDbContext.SaveChangesAsync();

            try
            {
                PublishResponse result = await _smsService.SendAsync(notification.Receivers, template.Title, content);
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
