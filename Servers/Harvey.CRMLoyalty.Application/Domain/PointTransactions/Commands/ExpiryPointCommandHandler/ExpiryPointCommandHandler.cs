using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.ExpiryPointCommandHandler
{
    public class ExpiryPointCommandHandler : IExpiryPointCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;
        const string BasicMembershipType = "Basic";
        const string ExpiryPoint = "Expiry Point";
        const string SystemAutomatic = "System Automatic";
        const string SystemAutomaticId = "af3dd4d8-26ec-46c4-a65e-b37272e68319";
        public ExpiryPointCommandHandler(HarveyCRMLoyaltyDbContext dbContext, ILoggingActivityService loggingActivityService, IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<decimal> ExecuteAsync(ExpiryPointCommand ec)
        {
            var currentDate = ec.Date;
            var listExpiryTransaction = GetExpiryTransaction(currentDate.Date);
            var listCustomerId = listExpiryTransaction.Select(x => x.CustomerId).Distinct();

            foreach (var customerId in listCustomerId)
            {
                var lastTransactionCredit = GetLastTransactionCredit(customerId);
                var lastTransaction = GetLastTransaction(customerId);
                var balanceTotal = lastTransaction.BalanceTotal;
                if (balanceTotal > 0)
                {
                    if (lastTransactionCredit.ExpiredDate.Value.Date <= currentDate.Date)
                    {
                        var pointValue = GetExpiryPointsWithLastItemInPeriodTime(balanceTotal, customerId, currentDate);
                        if (pointValue != 0)
                        {
                            var transaction = BuildPointTransaction(customerId, pointValue, balanceTotal, lastTransaction.BalanceDebit, lastTransaction.BalanceCredit);
                            _dbContext.PointTransactions.Add(transaction);
                            await _dbContext.SaveChangesAsync();

                            LogAction(SystemAutomatic, _config.Value.RabbitMqUrl);
                        }
                    }
                    else
                    {
                        var pointValue = GetExpiryPointsWithLastItemOutPeriodTime(balanceTotal, customerId, currentDate);
                        if (pointValue != 0)
                        {
                            var transaction = BuildPointTransaction(customerId, pointValue, balanceTotal, lastTransaction.BalanceDebit, lastTransaction.BalanceCredit);
                            _dbContext.PointTransactions.Add(transaction);
                            await _dbContext.SaveChangesAsync();

                            LogAction(SystemAutomatic, _config.Value.RabbitMqUrl);
                        }
                    }
                }
            }

            return 0;
        }

        private PointTransaction BuildPointTransaction(string customerId, decimal pointValue, decimal availablePoint, decimal balanceDebit, decimal balanceCredit)
        {
            var transaction = new PointTransaction();
            transaction.Id = Guid.NewGuid().ToString();
            transaction.CustomerId = customerId;
            transaction.Debit = pointValue;
            transaction.StaffId = SystemAutomaticId;
            transaction.OutletId = null;
            transaction.PointTransactionTypeId = (int)PointTransactionTypeEnum.ExpiryPoint;
            transaction.BalanceTotal = availablePoint - pointValue;
            transaction.BalanceDebit = balanceDebit + pointValue;
            transaction.BalanceCredit = balanceCredit;
            transaction.Voided = false;
            transaction.UpdatedDate = DateTime.UtcNow;
            transaction.CreatedBy = SystemAutomaticId;
            transaction.CreatedDate = DateTime.UtcNow;
            transaction.IPAddress = SystemAutomatic;
            return transaction;
        }

        private decimal GetExpiryPointsWithLastItemInPeriodTime(decimal availablePoint, string customerId, DateTime currentDate)
        {
            decimal expiryPoint = 0;
            decimal tempAvailablePoints = availablePoint;
            var expiryTransactions = _dbContext.PointTransactions.AsNoTracking()
                       .Where(x => x.ExpiredDate.Value.Date == currentDate.Date
                       && x.CustomerId == customerId
                       && x.Credit != 0
                       && x.PointTransactionReferenceId == null
                       && !_dbContext.PointTransactions.Any(y => y.PointTransactionReferenceId == x.Id))
                       .GroupBy(o => o.ExpiredDate.Value.Date).SelectMany(x => x).ToList();

            foreach (var item in expiryTransactions)
            {
                if (tempAvailablePoints - item.Credit >= 0)
                {
                    expiryPoint += item.Credit;
                    tempAvailablePoints -= item.Credit;
                }
                else
                {
                    expiryPoint += tempAvailablePoints;
                    tempAvailablePoints -= item.Credit;
                    break;
                }
            }

            return expiryPoint;
        }

        private decimal GetExpiryPointsWithLastItemOutPeriodTime(decimal availablePoint, string customerId, DateTime currentDate)
        {
            decimal expiryPoint = 0;
            decimal tempAvailablePoints = availablePoint;

            var expiryTransactionsOutOfPeriodTime = _dbContext.PointTransactions.AsNoTracking()
                .Where(x => x.ExpiredDate.Value.Date > currentDate.Date
                && x.CustomerId == customerId
                && x.Credit != 0
                && x.PointTransactionReferenceId == null
                && !_dbContext.PointTransactions.Any(y => y.PointTransactionReferenceId == x.Id))
                .GroupBy(o => o.ExpiredDate.Value.Date)
                .Select(x => new
                {
                    expiry = x.Key,
                    pointTransactions = x
                })
                .ToList();

            for (int i = expiryTransactionsOutOfPeriodTime.Count - 1; i >= 0; i--)
            {
                if (tempAvailablePoints <= 0)
                {
                    expiryPoint = 0;
                    break;
                }
                foreach (var item in expiryTransactionsOutOfPeriodTime[i].pointTransactions)
                {
                    tempAvailablePoints -= item.Credit;
                }
            }

            if (tempAvailablePoints > 0)
            {
                expiryPoint = GetExpiryPointsWithLastItemInPeriodTime(tempAvailablePoints, customerId, currentDate);
            }

            return expiryPoint;
        }

        public PointTransaction GetLastTransaction(string customerId)
        {
            var transaction = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return transaction;
        }

        public PointTransaction GetLastTransactionCredit(string customerId)
        {
            var transaction = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId && b.Credit != 0).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return transaction;
        }

        private List<PointTransaction> GetExpiryTransaction(DateTime date)
        {
            return _dbContext.PointTransactions.Where(a => a.ExpiredDate.Value.Date == date.Date).ToList();
        }

        private async void LogAction(string userId, string rabbitMqUrl)
        {
            var request = new LoggingActivityRequest();
            request.UserId = SystemAutomatic;
            request.Description = ExpiryPoint;
            request.Comment = SystemAutomatic;
            request.ActionType = ActionType.ExpiryPoint;
            request.ActionAreaPath = ActionArea.Job;
            request.CreatedByName = SystemAutomatic;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
