using System;
using System.Threading.Tasks;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler.Model;
using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Linq;
using Harvey.CRMLoyalty.Application.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Domain.AppSettings.Queries.GetAppSettings;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler
{
    public class ExpiryRewardPointNotificationCommand : IExpiryRewardPointNotificationCommand
    {
        private const string ACRONYM_BRAND_TITLE_VALUE = "AcronymBrandTitleValue";
        private const string BRAND_HOME_LINK_URL = "BrandHomeLinkUrl";
        private const string PeriodTimeToReminderExpiryPoint = "PeriodTimeToReminderExpiryPoint";
        private const string HaveReminderExpiryPoint = "HaveReminderExpiryPoint";

        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private readonly IOptions<ConfigurationRabbitMq> _configurationRabbitMq;
        private readonly IBusControl _bus;
        private readonly IGetAppSettingsQuery _getAppSettingsQuery;

        public ExpiryRewardPointNotificationCommand(
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

        public async Task ExecuteAsync(ExpiryRewardPointNotificationModel command)
        {
            var appSettingNames = new List<string>()
            {
                ACRONYM_BRAND_TITLE_VALUE,
                BRAND_HOME_LINK_URL,
                PeriodTimeToReminderExpiryPoint,
                HaveReminderExpiryPoint
            };

            var reminderExpiryPointSettings = _getAppSettingsQuery.GetAppSettingsByListName(appSettingNames).AppSettingModels;
            bool haveReminderExriryPoint;
            bool.TryParse(reminderExpiryPointSettings.FirstOrDefault(x => x.Name == HaveReminderExpiryPoint).Value, out haveReminderExriryPoint);
            if (haveReminderExriryPoint)
            {
                string AcronymBrandTitle = reminderExpiryPointSettings.FirstOrDefault(x => x.Name == ACRONYM_BRAND_TITLE_VALUE).Value.ToString();
                string BrandHomeUrl = reminderExpiryPointSettings.FirstOrDefault(x => x.Name == BRAND_HOME_LINK_URL).Value.ToString();

                int periodDayToReminder;
                int.TryParse(reminderExpiryPointSettings.FirstOrDefault(x => x.Name == PeriodTimeToReminderExpiryPoint).Value, out periodDayToReminder);
                DateTime expiryDate = command.Date.AddDays(periodDayToReminder);
                List<CustomerIncludeExpiryRewardPoint> customersIncludeExpiryRewardPointList = new List<CustomerIncludeExpiryRewardPoint>();


                var customer = _dbContext.PointTransactions.Where(x => x.ExpiredDate.Value.Date == expiryDate.Date).Select(x => new { id = x.CustomerId, createdDate = x.CreatedDate.Value.Date }).Distinct().ToList();
                var transactionsPointByCustomer = _dbContext.PointTransactions.Where(x => x.PointTransactionTypeId == (int)PointTransactionTypeEnum.AddPoint && customer.Exists(a => a.id == x.CustomerId))
                                        .Include(x => x.Customer)
                                        .GroupBy(x => x.CustomerId)
                                        .Select(group => new
                                        {
                                            customerId = group.Key,
                                            customerPhone = group.FirstOrDefault().Customer.Phone,
                                            totalPointCreatedInDate = group.Where(x => x.CreatedDate.Value.Date == customer.Where(a => a.id == group.Key).Select(b => b.createdDate).FirstOrDefault()).Sum(x => x.Credit),
                                            totalPointCreatedAfterDate = group.Where(x => x.CreatedDate.Value.Date <= expiryDate.Date && x.CreatedDate.Value.Date > customer.Where(a => a.id == group.Key).Select(b => b.createdDate).FirstOrDefault()).Sum(x => x.Credit),
                                            currentBalanceTotal = _dbContext.PointTransactions.AsNoTracking().Where(a => a.CustomerId == group.Key)
                                                                                    .OrderByDescending(x => x.CreatedDate)
                                                                                    .Select(a => a.BalanceTotal)
                                                                                    .FirstOrDefault(),
                                        }).ToList();


                foreach (var transactionPointByCustomer in transactionsPointByCustomer)
                {
                    if (transactionPointByCustomer.currentBalanceTotal - transactionPointByCustomer.totalPointCreatedAfterDate > 0)
                    {
                        CustomerIncludeExpiryRewardPoint customerIncludeExpiryRewardPoint = new CustomerIncludeExpiryRewardPoint
                        {
                            ExpiringPoints = (transactionPointByCustomer.currentBalanceTotal - transactionPointByCustomer.totalPointCreatedAfterDate) > transactionPointByCustomer.totalPointCreatedInDate
                                                                                    ? transactionPointByCustomer.totalPointCreatedInDate
                                                                                    : transactionPointByCustomer.currentBalanceTotal - transactionPointByCustomer.totalPointCreatedAfterDate,
                            ExpiredDate = expiryDate,
                            Phone = transactionPointByCustomer.customerPhone,
                            BrandHomeLinkUrl = BrandHomeUrl,
                            AcronymBrandTitle = AcronymBrandTitle
                        };
                        customersIncludeExpiryRewardPointList.Add(customerIncludeExpiryRewardPoint);
                    }

                }

                ISendEndpoint sendSmsExpiryRewardPointNotificationPoint = await _bus.GetSendEndpoint(new Uri(string.Concat(_configurationRabbitMq.Value.RabbitMqUrl, "/", "send_sms_expiry_reward_point_notification_queue")));
                await sendSmsExpiryRewardPointNotificationPoint.Send<SendSmsExpiryRewardPointNotificationCommand>(new
                {
                    CustomersIncludeExpiryRewardPoint = customersIncludeExpiryRewardPointList
                });
            }
           
        }
    }
}
