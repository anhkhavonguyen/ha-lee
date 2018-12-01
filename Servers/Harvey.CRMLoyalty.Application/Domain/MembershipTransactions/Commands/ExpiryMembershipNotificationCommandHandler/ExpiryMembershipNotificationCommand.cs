using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler.Model;
using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Linq;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings;
using Harvey.CRMLoyalty.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.ExpiryMembershipNotificationCommandHandler
{
    public class ExpiryMembershipNotificationCommand : IExpiryMembershipNotificationCommand
    {
        private const string HAVE_REMINDER_EXPIRY_DATE = "HaveReminderExpiryMembership";
        private const string PERIOD_TIME_TO_REMINDER = "PeriodTimeToReminderExpiryMembership";
        private const string ACRONYM_BRAND_TITLE_VALUE = "AcronymBrandTitleValue";
        private const string BRAND_HOME_LINK_URL = "BrandHomeLinkUrl";
        
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private readonly IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IBusControl _bus;
        private readonly IGetAppSettingsQuery _getAppSettingsQuery;

        public ExpiryMembershipNotificationCommand(
            HarveyCRMLoyaltyDbContext dbContext,
            IOptions<ConfigurationRabbitMq> configurationRabbitMq,
            IBusControl bus,
            IGetAppSettingsQuery getAppSettingsQuery
            )
        {
            _dbContext = dbContext;
            _configurationRabbitMq = configurationRabbitMq;
            _bus = bus;
            _getAppSettingsQuery = getAppSettingsQuery;
        }

        public async Task ExecuteAsync(ExpiryMembershipNotificationModel command)
        {
            var appSettingNames = new List<string>()
            {
                HAVE_REMINDER_EXPIRY_DATE,
                PERIOD_TIME_TO_REMINDER,
                ACRONYM_BRAND_TITLE_VALUE,
                BRAND_HOME_LINK_URL
            };
            var reminderExpiryMembershipSettings = _getAppSettingsQuery.GetAppSettingsByListName(appSettingNames).AppSettingModels;

            var haveReminderExpiryTimeText = reminderExpiryMembershipSettings.FirstOrDefault(x => x.Name == HAVE_REMINDER_EXPIRY_DATE).Value;
            bool haveReminderExpiryTime;
            if (!bool.TryParse(haveReminderExpiryTimeText, out haveReminderExpiryTime))
            {
                return;
            }

            if(!haveReminderExpiryTime)
            {
                return;
            }

            int periodMonthToReminder;
            if (!int.TryParse(reminderExpiryMembershipSettings.FirstOrDefault(x => x.Name == PERIOD_TIME_TO_REMINDER).Value, out periodMonthToReminder))
            {
                return;
            }

            var periodDayToReminder = 30 * periodMonthToReminder;
            var acronymBrandTitle = reminderExpiryMembershipSettings.FirstOrDefault(x => x.Name == ACRONYM_BRAND_TITLE_VALUE).Value.ToString();
            var brandHomeLinkUrl = reminderExpiryMembershipSettings.FirstOrDefault(x => x.Name == BRAND_HOME_LINK_URL).Value.ToString();
            var expiryDateCondition = command.Date.AddDays(periodDayToReminder).Date;
            var expiryMemberships = _dbContext.MembershipTransactions
                                                .Where(x => x.MembershipTypeId == 2 
                                                && x.ExpiredDate != null && x.ExpiredDate.Value.Date >= expiryDateCondition)
                                                .Include(x => x.Customer)
                                                .GroupBy(x => x.CustomerId)
                                                .Select(group => group.OrderByDescending(x => x.CreatedDate).FirstOrDefault())
                                                .Where(x => x.ExpiredDate != null && x.ExpiredDate.Value.Date == expiryDateCondition)
                                                .Distinct()
                                                .Select(x => new ExpiryMembership
                                                {
                                                    AcronymBrandTitle = acronymBrandTitle,
                                                    BrandHomeLinkUrl = brandHomeLinkUrl,
                                                    Phone = x.Customer.Phone,
                                                    ExpiredMembershipDate = x.ExpiredDate.Value.Date.AddDays(1)
                                                }).ToList();
                                       
            if (!expiryMemberships.Any())
            {
                return;
            }
            
            ISendEndpoint sendSmsExpiryMembershipNotificationEndpoint = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "send_sms_expiry_membership_notification_queue")));
            await sendSmsExpiryMembershipNotificationEndpoint.Send<SendSmsExpiryMembershipNotificationCommand>(new
            {
                ExpiryMemberships = expiryMemberships
            });
        }
    }
}
