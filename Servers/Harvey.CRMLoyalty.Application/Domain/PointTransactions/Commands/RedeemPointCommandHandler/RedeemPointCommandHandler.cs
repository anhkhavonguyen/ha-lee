using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.RedeemPointCommandHandler
{
    public class RedeemPointCommandHandler : IRedeemPointCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;
        const string BasicMembershipType = "Basic";

        public RedeemPointCommandHandler(HarveyCRMLoyaltyDbContext dbContext, ILoggingActivityService loggingActivityService, IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<decimal> ExecuteAsync(RedeemPointCommand redeemPointCommand)
        {
            if (redeemPointCommand == null)
                return -1;
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == redeemPointCommand.CustomerId && x.Status == Entities.Status.Active);
            if (checkExistCustomer)
            {
                var membershipType = GetMembershipType(redeemPointCommand.CustomerId);
                if (membershipType == BasicMembershipType)
                    return -1;
                var balanceTotal = CalculatePointBalanceTotal(redeemPointCommand.CustomerId);
                if (balanceTotal - redeemPointCommand.Value < 0)
                    return -1;
                var entity = new Entities.PointTransaction();
                entity.Id = Guid.NewGuid().ToString();
                entity.CustomerId = redeemPointCommand.CustomerId;
                entity.Debit = redeemPointCommand.Value;
                entity.StaffId = redeemPointCommand.UserId;
                entity.OutletId = redeemPointCommand.OutletId;
                entity.PointTransactionTypeId = (int)PointTransactionTypeEnum.RedeemPoint;
                entity.BalanceDebit = CalculatePointBalanceDebit(redeemPointCommand.CustomerId, redeemPointCommand.Value);
                entity.BalanceCredit = CalculatePointBalanceCredit(redeemPointCommand.CustomerId, 0);
                entity.BalanceTotal = entity.BalanceCredit - entity.BalanceDebit;
                entity.Voided = false;
                entity.UpdatedDate = DateTime.UtcNow;
                entity.CreatedBy = redeemPointCommand.UserId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.IPAddress = redeemPointCommand.IpAddress;
                _dbContext.PointTransactions.Add(entity);
                await _dbContext.SaveChangesAsync();

                var customer = await _dbContext.Customers.FindAsync(redeemPointCommand.CustomerId);
                var customerCode = "";
                var phoneNumber = "";
                if (customer != null)
                {
                    customer.LastUsed = DateTime.UtcNow;
                    customerCode = customer.CustomerCode;
                    phoneNumber = customer.PhoneCountryCode + customer.Phone;
                }
                await _dbContext.SaveChangesAsync();

                var user = _dbContext.Staffs.Where(a => a.Id == redeemPointCommand.UserId).FirstOrDefault();
                var userName = redeemPointCommand.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
                await LogAction(redeemPointCommand.UserId, _config.Value.RabbitMqUrl, customerCode, phoneNumber, userName);

                return entity.BalanceTotal;
            }
            return -1;
        }

        private decimal CalculatePointBalanceDebit(string customerId, decimal value)
        {
            var transactions = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId && b.Credit == 0).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();

            return transactions != null ? transactions.BalanceDebit + value : 0 + value;
        }

        private decimal CalculatePointBalanceCredit(string customerId, decimal value)
        {
            var transactions = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId && b.Debit == 0).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();

            return transactions != null ? transactions.BalanceCredit + value : 0 + value;
        }

        private decimal CalculatePointBalanceTotal(string customerId)
        {
            var transactions = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return transactions != null ? transactions.BalanceTotal : 0;
        }

        private string GetMembershipType(string customerId)
        {
            var membership = _dbContext.MembershipTransactions.Where(m => m.CustomerId == customerId).Include(m => m.MembershipType).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return membership != null ? membership.MembershipType.TypeName : "";
        }


        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.RedeemPoint;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
