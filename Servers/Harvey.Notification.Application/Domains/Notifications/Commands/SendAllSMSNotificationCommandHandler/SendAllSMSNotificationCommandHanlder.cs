using Harvey.Notification.Api;
using Harvey.Notification.Application.Data;
using Harvey.Notification.Application.Services.SMSService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;

namespace Harvey.Notification.Application.Domains.Notifications.Commands.SendAllSMSNotificationCommandHandler
{
    internal class SendAllSMSNotificationCommandHanlder : ISendAllSMSNotificationCommandHanlder
    {
        private readonly HarveyNotificationDbContext _harveyNotificationDbContext;
        private readonly ISMSService _smsService;

        public SendAllSMSNotificationCommandHanlder(HarveyNotificationDbContext harveyNotificationDbContext,
            ISMSService smsService)
        {
            _harveyNotificationDbContext = harveyNotificationDbContext;
            _smsService = smsService;
        }

        public void Execute()
        {
            try
            {
                int pendingStatus = (int)Status.Pending;
                int smsType = (int)NotifyType.Sms;
                var allPendingSMS = _harveyNotificationDbContext.Notifications.Where(w => w.Status == pendingStatus && w.NotificationType.Id == smsType).Include("Template").Include("NotificationType").ToList();
                if (allPendingSMS.Count > 0)
                {
                    allPendingSMS.ForEach(sms =>
                    {
                        var result = _smsService.SendAsync(sms.Receivers, sms.Template.Title, sms.Content).Result;
                        if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            sms.Status = (int)Status.Success;
                            _harveyNotificationDbContext.SaveChanges();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetBaseException().ToString());
            }
        }
    }
}
