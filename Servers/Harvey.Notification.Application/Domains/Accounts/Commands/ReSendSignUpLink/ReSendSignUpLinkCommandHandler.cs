using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Services.SMSService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Accounts.Commands.ReSendSignUpLink
{
    public class ReSendSignUpLinkCommandHandler : IReSendSignUpLinkCommandHandler
    {
        private readonly HarveyNotificationDbContext _dbContext;
        private readonly ISMSService _smsService;
        private const string SMS_INIT_ACCOUNT = "SMS_INIT_ACCOUNT";
        public ReSendSignUpLinkCommandHandler(HarveyNotificationDbContext dbContext, ISMSService smsService)
        {
            _dbContext = dbContext;
            _smsService = smsService;
        }
        public async Task ExecuteAsync(ReSendSignUpLinkCommandRequest request)
        {
            string phonenumber = $"+{request.CountryCode}{request.PhoneNumber}";

            var template = _dbContext.Templates.First(f => f.TemplateKey == TemplateConfig.SMS_INIT_ACCOUNT);
            string content = string.Format(template.Content, request.OutletName, request.PIN, request.SignUpShortLink);

            var notification = new Entities.Notification
            {
                NotificationTypeId = (int)NotifyType.Sms,
                TemplateId = template.Id,
                Content = content,
                Receivers = phonenumber,
                Status = (int)Status.Pending,
                Action = Entities.Action.ResendSignUp
            };
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            try
            {
                await _smsService.SendAsync(phonenumber, template.Title, content);
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetBaseException().ToString());
            }
        }
    }
}
