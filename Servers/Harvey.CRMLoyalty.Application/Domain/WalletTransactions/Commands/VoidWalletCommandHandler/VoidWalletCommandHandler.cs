using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Constants;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.VoidWalletCommandHandler
{
    public class VoidWalletCommandHandler : IVoidWalletCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;

        const string VoidWallet = "Void Wallet";
        public VoidWalletCommandHandler(
            HarveyCRMLoyaltyDbContext dbContext,
            ILoggingActivityService loggingActivityService,
            IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }
        public async Task<decimal> ExecuteAsync(VoidWalletCommand command)
        {
            decimal result = -1;
            if (command == null)
                return result;

            var walletTransaction = _dbContext.WalletTransactions.Include(p => p.Customer).FirstOrDefault(p => p.Id == command.WalletTransactionId);
            if (walletTransaction == null)
                return result;
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == walletTransaction.CustomerId && x.Status == Entities.Status.Active);
            if (checkExistCustomer)
            {
                var transactionType = walletTransaction.Credit == 0 ? WalletTransactionTypeEnum.Credit : WalletTransactionTypeEnum.Debit;
                result = CaculateVoidTransaction(ref walletTransaction, command, transactionType);

                if (result >= 0)
                {
                    var phoneNumber = walletTransaction.Customer?.PhoneCountryCode + walletTransaction.Customer?.Phone;
                    var user = _dbContext.Staffs.Where(a => a.Id == command.UserId).FirstOrDefault();
                    var userName = command.UserId == LogInformation.AdministratorId ? LogInformation.AdministratorName : (user != null ? $"{user.FirstName} {user.LastName}" : "");
                    await LogAction(command.UserId, _config.Value.RabbitMqUrl, walletTransaction.Customer?.CustomerCode, phoneNumber, userName);
                }
            }
            return result;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode + "-" + VoidWallet;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.Void;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }

        private decimal CaculateVoidTransaction(
            ref WalletTransaction voidedTransaction,
            VoidWalletCommand command,
            WalletTransactionTypeEnum transactionType)
        {
            var newTransaction = new WalletTransaction();

            newTransaction.Id = Guid.NewGuid().ToString();
            newTransaction.CustomerId = voidedTransaction.CustomerId;
            newTransaction.StaffId = null;
            newTransaction.OutletId = voidedTransaction.OutletId;
            newTransaction.WalletTransactionReferenceId = voidedTransaction.Id;
            newTransaction.Voided = true;
            newTransaction.CreatedDate = DateTime.UtcNow;
            newTransaction.IPAddress = command.IpAddress;
            newTransaction.CreatedBy = command.UserId;
            newTransaction.CreatedByName = command.VoidByName;

            var creditValue = transactionType == WalletTransactionTypeEnum.Credit
                ? voidedTransaction.Debit : 0;
            var debitValue = transactionType == WalletTransactionTypeEnum.Debit
                ? voidedTransaction.Credit : 0;

            newTransaction.Credit = creditValue;
            newTransaction.Debit = debitValue;

            newTransaction.BalanceCredit = CalculateBalanceCredit(voidedTransaction.CustomerId, creditValue);
            newTransaction.BalanceDebit = CalculateBalanceDebit(voidedTransaction.CustomerId, debitValue);
            newTransaction.BalanceTotal = newTransaction.BalanceCredit - newTransaction.BalanceDebit;

            if (newTransaction.BalanceTotal < 0)
                return -1;

            _dbContext.WalletTransactions.Add(newTransaction);
            _dbContext.SaveChanges();

            return newTransaction.BalanceTotal;
        }

        private decimal CalculateBalanceCredit(string customerId, decimal value)
        {
            var lastestTransaction = _dbContext.WalletTransactions
                .Where(b => b.CustomerId == customerId && b.Debit == 0)
                .OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return lastestTransaction != null ? lastestTransaction.BalanceCredit + value : value;
        }

        private decimal CalculateBalanceDebit(string customerId, decimal value)
        {
            var transactions = _dbContext.WalletTransactions
                .Where(x => x.CustomerId == customerId && x.Credit == 0)
                .OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return transactions != null ? transactions.BalanceDebit + value : value;
        }
    }

    public enum WalletTransactionTypeEnum
    {
        Credit,
        Debit
    }
}
