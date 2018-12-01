using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions.Model;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetMembershipTransactions
{
    public class GetMembershipTransactions : IGetMembershipTransactions
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetMembershipTransactions(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GetMembershipTransactionsResponse Execute(GetMembershipTransactionsRequest request)
        {
            var query = _dbContext.MembershipTransactions.AsNoTracking()
               .Where(x => x.OutletId == request.OutletId)
               .Select(o => new MembershipTransactionModel
               {
                   Id = o.Id,
                   MembershipType = o.MembershipType.TypeName,
                   Comment = o.Comment,
                   CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                   OutletName = o.Outlet.Name,
                   StaffName = o.Staff.FirstName + " " + o.Staff.LastName,
                   ExpiredDate = o.ExpiredDate,
                   PhoneCustomer = o.Customer.Phone,
                   PhoneCountryCode = o.Customer.PhoneCountryCode,
                   CreatedDate = o.CreatedDate,
                   CustomerCode = o.Customer.CustomerCode,
                   MembershipActionType = o.MembershipActionTypeId
               })
               .OrderByDescending(x => x.CreatedDate).AsQueryable();
            var result = PagingExtensions.GetPaged<MembershipTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetMembershipTransactionsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListMembershipTransaction = result.Results;
            return response;
        }
    }
}
