using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Commands.VoidMembershipCommandHandler
{
    public class VoidMembershipCommandHandler : IVoidMembershipCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        private ILoggingActivityService _loggingActivityService;
        private IOptions<ConfigurationRabbitMq> _config;
        const string VoidMembership = "Void Membership";
        public VoidMembershipCommandHandler(
            HarveyCRMLoyaltyDbContext dbContext,
            ILoggingActivityService loggingActivityService,
            IOptions<ConfigurationRabbitMq> config)
        {
            _dbContext = dbContext;
            _config = config;
            _loggingActivityService = loggingActivityService;
        }

        public async Task<string> ExecuteAsync(VoidMembershipCommand command)
        {
            if (command == null)
                return null;           
            var membershipTransaction = new Entities.MembershipTransaction();
            var voidedTransaction = _dbContext.MembershipTransactions.Include(x => x.Customer).Where(x => x.Id == command.MembershipTransactionId).FirstOrDefault();
            if (voidedTransaction != null)
            {
                var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == voidedTransaction.CustomerId && x.Status == Entities.Status.Active);
                if (checkExistCustomer)
                {
                    var lastedMembershipTransaction = _dbContext.MembershipTransactions.Where(x =>
                        x.CustomerId == voidedTransaction.CustomerId
                        && x.CreatedDate < voidedTransaction.CreatedDate
                        && x.MembershipTransactionReference == null
                        && !_dbContext.MembershipTransactions.Any(y => y.MembershipTransactionReferenceId == x.Id)
                    ).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    if (lastedMembershipTransaction != null)
                    {
                        membershipTransaction.Id = Guid.NewGuid().ToString();
                        membershipTransaction.MembershipTypeId = lastedMembershipTransaction.MembershipTypeId;
                        membershipTransaction.ExpiredDate = lastedMembershipTransaction.ExpiredDate;
                        membershipTransaction.CustomerId = lastedMembershipTransaction.CustomerId;
                        membershipTransaction.CreatedBy = command.UserId;
                        membershipTransaction.CreatedByName = command.VoidByName;
                        membershipTransaction.CreatedDate = DateTime.UtcNow;
                        membershipTransaction.Comment = lastedMembershipTransaction.Comment;
                        membershipTransaction.MembershipTransactionReferenceId = command.MembershipTransactionId;
                        membershipTransaction.OutletId = voidedTransaction.OutletId;
                        membershipTransaction.IPAddress = command.IpAddress;
                        membershipTransaction.MembershipActionTypeId = command.MembershipActionType;
                        _dbContext.MembershipTransactions.Add(membershipTransaction);
                        await _dbContext.SaveChangesAsync();
                        var phoneNumber = voidedTransaction.Customer?.PhoneCountryCode + voidedTransaction.Customer?.Phone;
                        await LogAction(command.UserId, _config.Value.RabbitMqUrl, voidedTransaction.Customer?.CustomerCode, phoneNumber, command.VoidByName);
                        return membershipTransaction.Id;
                    }
                } 
            }    
            return null;
        }

        private async Task LogAction(string userId, string rabbitMqUrl, string customerCode, string phoneNumber, string userName)
        {
            var request = new LoggingActivityRequest();
            request.UserId = userId;
            request.Description = customerCode + "-" + VoidMembership;
            request.Comment = phoneNumber;
            request.ActionType = ActionType.Void;
            request.ActionAreaPath = ActionArea.AdminApp;
            request.CreatedByName = userName;
            await _loggingActivityService.ExecuteAsync(request, rabbitMqUrl);
        }
    }
}
