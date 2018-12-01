using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries
{
    public class GetMembershipTransactionsByCustomerQuery : IGetMembershipTransactionsByCustomerQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetMembershipTransactionsByCustomerQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetMembershipTransactionsByCustomerResponse Execute(GetMembershipTransactionsByCustomerRequest request)
        {
            var voidMembership = new Entities.MembershipTransaction();
            var firstMembership = _dbContext.MembershipTransactions.Where(x => x.CustomerId == request.CustomerId).OrderBy(x => x.CreatedDate).FirstOrDefault();
            voidMembership = _dbContext.MembershipTransactions.Count(x => x.CustomerId == request.CustomerId) > 1 ?
                _dbContext.MembershipTransactions
                    .Where(x =>
                        x.CustomerId == request.CustomerId
                        && x.MembershipTransactionReference == null
                        && !_dbContext.MembershipTransactions.Any(y => y.MembershipTransactionReferenceId == x.Id))
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault() :
                null;
            var voidMembershipId = voidMembership != null && firstMembership != null && voidMembership.Id != firstMembership.Id ? voidMembership.Id : "";
            var query = _dbContext.MembershipTransactions.AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId
                            && x.MembershipTransactionReferenceId == null)
                .Select(o => new MembershipTransactionModel{
                    Id = o.Id,
                    MembershipType = o.MembershipType.TypeName,
                    Comment = o.Comment,
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                    OutletName = o.Outlet.Name,
                    DoneBy = !string.IsNullOrEmpty(o.CreatedByName) ? o.CreatedByName : (o.Staff != null ? o.Staff.FirstName + " " + o.Staff.LastName : ""),
                    ExpiredDate = o.ExpiredDate,
                    PhoneCustomer = o.Customer.Phone,
                    PhoneCountryCode = o.Customer.PhoneCountryCode,
                    CreatedDate = o.CreatedDate,
                    AllowVoid = !string.IsNullOrEmpty(voidMembershipId) ? o.Id == voidMembershipId : false,
                    IPAddress = o.IPAddress,
                    Voided = _dbContext.MembershipTransactions.Any(x => x.MembershipTransactionReferenceId == o.Id) ? true : false,
                    VoidedBy = _dbContext.MembershipTransactions
                                .Any(x => x.MembershipTransactionReferenceId == o.Id)
                                ? _dbContext.MembershipTransactions
                                .FirstOrDefault(x => x.MembershipTransactionReferenceId == o.Id)
                                .CreatedByName
                                : "",
                    MembershipActionType = o.MembershipActionTypeId
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();
            var result = PagingExtensions.GetPaged<MembershipTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetMembershipTransactionsByCustomerResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListMembershipTransaction = result.Results;
            return response;
        }
    }
}
