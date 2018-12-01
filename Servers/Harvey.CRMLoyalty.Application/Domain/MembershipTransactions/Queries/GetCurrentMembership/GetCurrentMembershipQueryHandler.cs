using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetCurrentMembership.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetCurrentMembership
{
    internal class GetCurrentMembershipQueryHandler : IGetCurrentMembershipQueryHandler
    {
        private readonly HarveyCRMLoyaltyDbContext _harveyCRMLoyaltyDbContext;

        public GetCurrentMembershipQueryHandler(HarveyCRMLoyaltyDbContext harveyCRMLoyaltyDbContext)
        {
            _harveyCRMLoyaltyDbContext = harveyCRMLoyaltyDbContext;
        }

        public async Task<GetMembershipTransactionResponse> ExecuteAsync(string customerId)
        {
            var membershipTransaction = new GetMembershipTransactionResponse();
            var membership = await _harveyCRMLoyaltyDbContext.MembershipTransactions.AsNoTracking().Where(w => w.CustomerId == customerId).Include(o => o.MembershipType).OrderByDescending(o => o.CreatedDate).FirstOrDefaultAsync();
            if (membership != null)
            {
                membershipTransaction.Membership = membership.MembershipType.TypeName;
                membershipTransaction.ExpiredDate = membership.ExpiredDate;

                return membershipTransaction;
            } 
          
            return null;
        }
    }
}
