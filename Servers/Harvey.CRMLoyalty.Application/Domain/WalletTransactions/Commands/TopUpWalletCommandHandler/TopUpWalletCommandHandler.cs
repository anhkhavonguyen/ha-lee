using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.TopUpWalletCommandHandler
{
    public class TopUpWalletCommandHandler : ITopUpWalletCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;

        public TopUpWalletCommandHandler(HarveyCRMLoyaltyDbContext dbContext, ILoggingActivityService loggingActivityService, IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<decimal> ExecuteAsync(TopUpWalletCommand topUpWalletCommand)
        {
            if (topUpWalletCommand == null)
                return -1;
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == topUpWalletCommand.CustomerId && x.Status == Entities.Status.Active);
            if (checkExistCustomer)
            {
                var entity = new Entities.WalletTransaction();
                entity.Id = Guid.NewGuid().ToString();
                entity.StaffId = topUpWalletCommand.UserId;
                entity.Credit = topUpWalletCommand.Value;

                entity.CustomerId = topUpWalletCommand.CustomerId;
                entity.OutletId = topUpWalletCommand.OutletId;
                entity.BalanceCredit = CalculateWalletBalanceCredit(topUpWalletCommand.CustomerId, topUpWalletCommand.Value);
                entity.BalanceDebit = CalculateWalletBalanceDebit(topUpWalletCommand.CustomerId, 0);
                entity.BalanceTotal = entity.BalanceCredit - entity.BalanceDebit;
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = topUpWalletCommand.UserId;
                entity.IPAddress = topUpWalletCommand.IpAddress;

                _dbContext.WalletTransactions.Add(entity);
                await _dbContext.SaveChangesAsync();

                var customer = await _dbContext.Customers.FindAsync(topUpWalletCommand.CustomerId);
                var customerCode = "";
                var phoneNumber = "";
                if (customer != null)
                {
                    customer.LastUsed = DateTime.UtcNow;
                    customerCode = customer.CustomerCode;
                    phoneNumber = customer.PhoneCountryCode + customer.Phone;
                }
                await _dbContext.SaveChangesAsync();
                var user = _dbContext.Staffs.Where(a => a.Id == topUpWalletCommand.UserId).FirstOrDefault();
                var userName = topUpWalletCommand.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
                await LogAction(topUpWalletCommand.UserId, _config.Value.RabbitMqUrl, customerCode, phoneNumber, userName);

                return entity.BalanceTotal;
            }
            return -1;
        }

        private decimal CalculateWalletBalanceCredit(string customerId, decimal value)
        {
            var transactions = _dbContext.WalletTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return transactions != null ? transactions.BalanceCredit + value : 0 + value;
        }

        private decimal CalculateWalletBalanceDebit(string customerId, decimal value)
        {
            var transactions = _dbContext.WalletTransactions.Where(b => b.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return transactions != null ? transactions.BalanceDebit + value : 0;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.TopUp;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
