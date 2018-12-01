using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.VoidPointCommandHandler
{
    public class VoidPointCommandHandler : IVoidPointCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;

        const string VoidPoint = "Void Point";

        public VoidPointCommandHandler(HarveyCRMLoyaltyDbContext dbContext, ILoggingActivityService loggingActivityService, IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }
        public async Task<decimal> ExecuteAsync(VoidPointCommand command)
        {
            decimal result = -1;
            if (command == null)
                return result;

            var pointTransaction = _dbContext.PointTransactions.Include(p => p.Customer).FirstOrDefault(p => p.Id == command.PointTransactionId);
            if (pointTransaction == null)
                return result;
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == pointTransaction.CustomerId && x.Status == Entities.Status.Active);
            if (checkExistCustomer)
            {
                if (pointTransaction.Credit > 0)
                {
                    if (pointTransaction.ExpiredDate.HasValue && pointTransaction.ExpiredDate.Value >= DateTime.UtcNow)
                        result = CaculateVoidCreditTransaction(ref pointTransaction, command);
                }
                else
                {
                    result = CaculateVoidDebitTransaction(ref pointTransaction, command);
                }

                if (result >= 0)
                {
                    var phoneNumber = pointTransaction.Customer?.PhoneCountryCode + pointTransaction.Customer?.Phone;
                    var user = _dbContext.Staffs.Where(a => a.Id == command.UserId).FirstOrDefault();
                    var userName = command.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
                    await LogAction(command.UserId, _config.Value.RabbitMqUrl, pointTransaction.Customer.CustomerCode, phoneNumber, userName);
                }
            }
            return result;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode + "-" + VoidPoint;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.Void;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }

        private decimal CaculateVoidCreditTransaction(ref PointTransaction entity, VoidPointCommand command)
        {
            var newEntity = new PointTransaction();

            newEntity.Id = Guid.NewGuid().ToString();
            newEntity.CustomerId = entity.CustomerId;
            newEntity.StaffId = entity.StaffId;
            newEntity.OutletId = entity.OutletId;
            newEntity.PointTransactionReferenceId = entity.Id;

            newEntity.PointTransactionTypeId = (int)PointTransactionTypeEnum.Void;
            newEntity.Voided = true;
            newEntity.ExpiredDate = null;
            newEntity.CreatedDate = DateTime.UtcNow;
            newEntity.IPAddress = command.IpAddress;
            newEntity.CreatedBy = command.UserId;
            newEntity.CreatedByName = command.VoidByName;

            newEntity.Credit = 0;
            newEntity.Debit = entity.Credit;

            newEntity.BalanceCredit = CalculateBalanceCredit(entity.CustomerId, 0);
            newEntity.BalanceDebit = CalculateBalanceDebit(entity.CustomerId, entity.Credit);
            newEntity.BalanceTotal = CalculateBalanceTotal(entity.CustomerId, newEntity.BalanceCredit, newEntity.BalanceDebit);

            if (newEntity.BalanceTotal < 0)
                return -1;

            _dbContext.PointTransactions.Add(newEntity);
            _dbContext.SaveChanges();

            return newEntity.BalanceTotal;
        }

        private decimal CaculateVoidDebitTransaction(ref PointTransaction entity, VoidPointCommand command)
        {
            decimal result = -1;
            var createdDateOfVoidTrans = entity.CreatedDate;
            var customerId = entity.CustomerId;
            var beforeTransaction = _dbContext.PointTransactions
                .OrderByDescending(x => x.CreatedDate)
                .Where(x => x.CustomerId == customerId && x.CreatedDate.HasValue && x.CreatedDate.Value < createdDateOfVoidTrans.Value)
                .FirstOrDefault();
            if (beforeTransaction != null && beforeTransaction.BalanceTotal > 0)
            {
                var listCreditTransaction = _dbContext.PointTransactions
                    .Where(x => x.CustomerId == customerId
                        && x.Credit > 0
                        && x.CreatedDate.HasValue
                        && x.CreatedDate.Value < createdDateOfVoidTrans.Value
                        && x.PointTransactionReferenceId == null
                        && (x.VoidPointTransactions == null || x.VoidPointTransactions.Count == 0))
                    .OrderByDescending(x => x.CreatedDate).ToList();
                if (listCreditTransaction.Count() > 0)
                {
                    var listAvalablePoint = new List<PointTransaction>();
                    var balanceTotal = beforeTransaction.BalanceTotal;
                    decimal remainingBalance = 0;
                    foreach (PointTransaction p in listCreditTransaction)
                    {
                        listAvalablePoint.Add(p);
                        balanceTotal -= p.Credit;
                        if (balanceTotal == 0)
                            break;
                        else if (balanceTotal < 0)
                        {
                            remainingBalance = balanceTotal + p.Credit;
                            break;
                        }
                    }
                    listAvalablePoint = listAvalablePoint.OrderBy(x => x.ExpiredDate).ToList();
                    if (listAvalablePoint.Count > 0 && balanceTotal < 0)
                    {
                        listAvalablePoint[0].Credit = remainingBalance;
                    }
                    var valueDebit = entity.Debit;
                    var lastestTransaction = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
                    var totalBalanceDebit = lastestTransaction.BalanceDebit;
                    var totalBalanceCredit = lastestTransaction.BalanceCredit;
                    var listVoidPointTrasaction = new List<PointTransaction>();
                    foreach (PointTransaction p in listAvalablePoint)
                    {
                        if (p.ExpiredDate.HasValue && p.ExpiredDate.Value < DateTime.UtcNow)
                            return result;
                        var newEntity = new PointTransaction();
                        newEntity.Id = Guid.NewGuid().ToString();
                        newEntity.CustomerId = entity.CustomerId;
                        newEntity.StaffId = entity.StaffId;
                        newEntity.OutletId = entity.OutletId;
                        newEntity.PointTransactionReferenceId = entity.Id;

                        newEntity.PointTransactionTypeId = (int)PointTransactionTypeEnum.Void;
                        newEntity.Voided = true;
                        newEntity.ExpiredDate = p.ExpiredDate;
                        newEntity.CreatedDate = DateTime.UtcNow;
                        newEntity.IPAddress = command.IpAddress;
                        newEntity.CreatedBy = command.UserId;
                        newEntity.CreatedByName = command.VoidByName;

                        newEntity.Debit = 0;
                        if (valueDebit - p.Credit > 0)
                        {
                            newEntity.Credit = p.Credit;
                        }
                        else
                        {
                            newEntity.Credit = valueDebit;
                        }
                        newEntity.BalanceDebit = totalBalanceDebit;
                        totalBalanceCredit += newEntity.Credit;
                        newEntity.BalanceCredit = totalBalanceCredit;
                        newEntity.BalanceTotal = newEntity.BalanceCredit - newEntity.BalanceDebit;
                        listVoidPointTrasaction.Add(newEntity);
                        valueDebit -= p.Credit;
                        result = newEntity.BalanceTotal;
                        if (valueDebit <= 0)
                        {
                            break;
                        }
                    }
                    if (listVoidPointTrasaction.Count > 0)
                    {
                        _dbContext.PointTransactions.AddRange(listVoidPointTrasaction);
                        _dbContext.SaveChanges();
                    }
                }
            }
            return result;
        }

        private decimal CalculateBalanceCredit(string customerId, decimal value)
        {
            var lastestTransaction = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId && b.Debit == 0).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return lastestTransaction != null ? lastestTransaction.BalanceCredit + value : 0 + value;
        }

        private decimal CalculateBalanceDebit(string customerId, decimal value)
        {
            var transactions = _dbContext.PointTransactions.Where(x => x.CustomerId == customerId && x.Credit == 0).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return transactions != null ? transactions.BalanceDebit + value : 0 + value;
        }

        private decimal CalculateBalanceTotal(string customerId, decimal creditBalance, decimal debitBalance)
        {
            return creditBalance - debitBalance;
        }
    }
}
