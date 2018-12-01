using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddPointCommandHandler
{
    public class AddPointCommandHandler : IAddPointCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;

        public AddPointCommandHandler(HarveyCRMLoyaltyDbContext dbContext,
            ILoggingActivityService loggingActivityService, IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<decimal> ExecuteAsync(AddPointCommand addPointCommand)
        {
            if (addPointCommand == null)
                return -1;
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == addPointCommand.CustomerId && x.Status == Entities.Status.Active);
            if (checkExistCustomer)
            {
                var entity = new Entities.PointTransaction();
                entity.Id = Guid.NewGuid().ToString();
                entity.CustomerId = addPointCommand.CustomerId;
                entity.Credit = addPointCommand.Value;
                entity.StaffId = addPointCommand.UserId;
                entity.OutletId = addPointCommand.OutletId;
                entity.PointTransactionTypeId = (int)PointTransactionTypeEnum.AddPoint;
                entity.BalanceCredit = CalculatePointBalanceCredit(addPointCommand.CustomerId, addPointCommand.Value);
                entity.BalanceDebit = CalculateBalanceDebit(addPointCommand.CustomerId, 0);
                entity.BalanceTotal = entity.BalanceCredit - entity.BalanceDebit;
                entity.Voided = false;
                entity.ExpiredDate = DateTime.UtcNow.AddYears(1);
                entity.UpdatedDate = DateTime.UtcNow;
                entity.CreatedBy = addPointCommand.UserId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.IPAddress = addPointCommand.IpAddress;
                _dbContext.PointTransactions.Add(entity);
                await _dbContext.SaveChangesAsync();

                var customer = await _dbContext.Customers.FindAsync(addPointCommand.CustomerId);
                var customerCode = "";
                var phoneNumber = "";
                if (customer != null)
                {
                    customer.LastUsed = DateTime.UtcNow;
                    customerCode = customer.CustomerCode;
                    phoneNumber = customer.PhoneCountryCode + customer.Phone;
                }
                await _dbContext.SaveChangesAsync();
                var user = _dbContext.Staffs.Where(a => a.Id == addPointCommand.UserId).FirstOrDefault();
                var userName = addPointCommand.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
                await LogAction(addPointCommand.UserId, _config.Value.RabbitMqUrl, customerCode, phoneNumber, userName);
                return entity.BalanceTotal;
            }
            return -1;
        }

        private decimal CalculatePointBalanceCredit(string customerId, decimal value)
        {
            var transactions = _dbContext.PointTransactions.Where(b => b.CustomerId == customerId && b.Debit == 0).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();

            return transactions != null ? transactions.BalanceCredit + value : 0 + value;
        }

        private decimal CalculateBalanceDebit(string customerId, decimal value)
        {
            var transactions = _dbContext.PointTransactions.Where(x => x.CustomerId == customerId && x.Credit == 0).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return transactions != null ? transactions.BalanceDebit + value : 0 + value;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.AddPoint;
            request.ActionAreaPath = ActionArea.StoreApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
