using Harvey.CRMLoyalty.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Commands.AddMembershipCommandHandler
{
    public class AddMembershipCommandHandler : IAddMembershipCommandHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public AddMembershipCommandHandler(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> ExecuteAsync(AddMembershipCommand addMembershipCommand)
        {
            var checkExistCustomer = _dbContext.Customers.Any(x => x.Id == addMembershipCommand.CustomerId && x.Status == Entities.Status.Active);
            if(checkExistCustomer)
            {
                var membershipTransaction = new Entities.MembershipTransaction();

                membershipTransaction.Id = Guid.NewGuid().ToString();
                membershipTransaction.CustomerId = addMembershipCommand.CustomerId;
                membershipTransaction.StaffId = addMembershipCommand.StaffId;
                membershipTransaction.OutletId = addMembershipCommand.OutletId;
                membershipTransaction.Comment = addMembershipCommand.Comment;
                membershipTransaction.MembershipTypeId = addMembershipCommand.MembershipTypeId;
                membershipTransaction.ExpiredDate = addMembershipCommand.ExpiredDate;
                membershipTransaction.UpdatedDate = DateTime.UtcNow;
                membershipTransaction.CreatedBy = addMembershipCommand.UserId;
                membershipTransaction.CreatedDate = DateTime.UtcNow;
                membershipTransaction.IPAddress = addMembershipCommand.IpAddress;
                membershipTransaction.MembershipActionTypeId = addMembershipCommand.MembershipActionType;

                _dbContext.MembershipTransactions.Add(membershipTransaction);

                var customer = await _dbContext.Customers.FindAsync(addMembershipCommand.CustomerId);
                if (customer != null)
                    customer.LastUsed = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return membershipTransaction.Id;
            }
            return null;      
        }
    }
}
