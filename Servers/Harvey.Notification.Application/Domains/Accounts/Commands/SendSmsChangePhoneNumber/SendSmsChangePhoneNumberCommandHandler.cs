using Amazon.SimpleNotificationService.Model;
using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Services.SMSService;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.SendSmsChangePhoneNumber
{
    public class SendSmsChangePhoneNumberCommandHandler : ISendSmsChangePhoneNumberCommandHandler
    {
        private readonly HarveyNotificationDbContext _dbContext;
        private readonly ISMSService _smsService;

        public SendSmsChangePhoneNumberCommandHandler(HarveyNotificationDbContext dbContext, ISMSService smsService)
        {
            _dbContext = dbContext;
            _smsService = smsService;
        }
        public async Task ExecuteAsync(SendSmsChangePhoneNumberCommandRequest command)
        {
            string phonenumber = $"+{command.NewPhoneCountryCode}{command.NewPhoneNumber}";
            var template = _dbContext.Templates.First(f => f.TemplateKey == TemplateConfig.SMS_SEND_CHANGE_MOBILE_PHONE);
            string content = string.Format(template.Content, command.AcronymBrandName, command.ResetPasswordShortLink, command.UpdateProfileShortLink, command.LoginShortLink);

            var notification = new Entities.Notification
            {
                NotificationTypeId = (int)NotifyType.Sms,
                TemplateId = template.Id,
                Content = content,
                Receivers = phonenumber,
                Status = (int)Status.Pending,
                Action = Entities.Action.ChangeMobilePhone
            };
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            try
            {
                PublishResponse result = await _smsService.SendAsync(notification.Receivers, template.Title, notification.Content);
                if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    notification.Status = (int)Status.Success;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetBaseException().ToString());
            }
        }
    }
}
