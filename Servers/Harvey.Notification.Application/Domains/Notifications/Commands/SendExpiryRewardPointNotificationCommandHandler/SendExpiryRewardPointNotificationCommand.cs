using Amazon.SimpleNotificationService.Model;
using Harvey.Notification.Api;
using Harvey.Notification.Application.Configs;
using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler.Model;
using Harvey.Notification.Application.Services.SMSService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler
{
    internal class SendExpiryRewardPointNotificationCommand : ISendExpiryRewardPointNotificationCommand
    {
        private readonly HarveyNotificationDbContext _dbContext;
        private readonly ISMSService _smsService;

        public SendExpiryRewardPointNotificationCommand(HarveyNotificationDbContext dbContext,
            ISMSService smsService)
        {
            _dbContext = dbContext;
            _smsService = smsService;
        }

        public async Task Execute(SendExpiryRewardPointNotificationRequest request)
        {
            var template = _dbContext.Templates.First(f => f.TemplateKey == TemplateConfig.SMS_SEND_EXPIRY_REWARD_POINT_NOTIFICATION);

            var expiryRewardPointsModels = request.SendCustomersIncludeExpiryRewardPointModel;
            if(expiryRewardPointsModels == null && !expiryRewardPointsModels.Any())
            {
                return;
            }

            var notificatons = new List<Entities.Notification>();
            foreach (var item in expiryRewardPointsModels)
            {
                var content = string.Format(template.Content, item.ExpiringPoints, item.ExpiredDate.ToString("dd-MMM-yyyy"), item.AcronymBrandTitle, item.BrandHomeLinkUrl);
                
                var notification = new Entities.Notification
                {
                    NotificationTypeId = (int)NotifyType.Sms,
                    TemplateId = template.Id,
                    Content = content,
                    Receivers = item.Phone,
                    Status = (int)Status.Pending,
                    Action = Entities.Action.ReminderExpiryRewardPoints
                };
                notificatons.Add(notification);
            }
            _dbContext.Notifications.AddRange(notificatons);
            await _dbContext.SaveChangesAsync();
            
            foreach (var notification in notificatons)
            {
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
}