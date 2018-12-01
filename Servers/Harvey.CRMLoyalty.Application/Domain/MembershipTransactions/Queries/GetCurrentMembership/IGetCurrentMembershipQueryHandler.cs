using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetCurrentMembership.Model;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetCurrentMembership
{
    public interface IGetCurrentMembershipQueryHandler
    {
        Task<GetMembershipTransactionResponse> ExecuteAsync(string customerId);
    }
}
