using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.SpendingWalletCommandHandler
{
    public class SpendingWalletCommandHandler : ISpendingWalletCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;
        const string Spending = "Spending";

        public SpendingWalletCommandHandler(HarveyCRMLoyaltyDbContext dbContext, ILoggingActivityService loggingActivityService, IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<decimal> ExecuteAsync(SpendingWalletCommand spendingWalletCommand)
        {
            if (spendingWalletCommand == null)
                return -1;
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == spendingWalletCommand.CustomerId && x.Status == Entities.Status.Active);
            if (checkExistCustomer)
            {
                var balanceTotal = CalculateWalletBalanceTotal(spendingWalletCommand.CustomerId);
                if (balanceTotal - spendingWalletCommand.Value < 0)
                    return -1;

                var entity = new Entities.WalletTransaction();
                entity.Id = Guid.NewGuid().ToString();
                entity.StaffId = spendingWalletCommand.StaffId;
                entity.Debit = spendingWalletCommand.Value;
                entity.CustomerId = spendingWalletCommand.CustomerId;
                entity.OutletId = spendingWalletCommand.OutletId;
                entity.BalanceDebit = CalculateWalletBalanceDebit(spendingWalletCommand.CustomerId, spendingWalletCommand.Value);
                entity.BalanceCredit = CalculateWalletBalanceCredit(spendingWalletCommand.CustomerId, 0);
                entity.BalanceTotal = entity.BalanceCredit - entity.BalanceDebit;
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = spendingWalletCommand.UserId;
                entity.IPAddress = spendingWalletCommand.IpAddress;
                entity.CreatedByName = spendingWalletCommand.CreatedByName;

                _dbContext.WalletTransactions.Add(entity);
                await _dbContext.SaveChangesAsync();

                var customer = await _dbContext.Customers.FindAsync(spendingWalletCommand.CustomerId);
                var customerCode = "";
                var phoneNumber = "";
                if (customer != null)
                {
                    customer.LastUsed = DateTime.UtcNow;
                    customerCode = customer.CustomerCode;
                    phoneNumber = customer.PhoneCountryCode + customer.Phone;
                }
                await _dbContext.SaveChangesAsync();
                var user = _dbContext.Staffs.Where(a => a.Id == spendingWalletCommand.UserId).FirstOrDefault();
                var userName = spendingWalletCommand.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
                await LogAction(spendingWalletCommand.UserId, _config.Value.RabbitMqUrl, customerCode, phoneNumber, userName);

                return entity.BalanceTotal;
            }
            return -1;    
        }

        private decimal CalculateWalletBalanceDebit(string customerId, decimal value)
        {
            var transactions = _dbContext.WalletTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();

            return transactions != null ? transactions.BalanceDebit + value : 0 + value;
        }

        private decimal CalculateWalletBalanceCredit(string customerId, decimal value)
        {
            var transactions = _dbContext.WalletTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();

            return transactions != null ? transactions.BalanceCredit + value : 0;
        }

        private decimal CalculateWalletBalanceTotal(string customerId)
        {
            var transactions = _dbContext.WalletTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return transactions != null ? transactions.BalanceTotal : 0;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.Spending;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
