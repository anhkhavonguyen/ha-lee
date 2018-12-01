using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class GetExpiredCustomersQuery : IGetExpiredCustomersQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetExpiredCustomersQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetExpiredCustomersResponse Execute(GetExpiredCustomersRequest request)
        {
            int premiumPlus = (int)MembershipTranactionEnum.PremiumPlus;

            var query = _dbContext.MembershipTransactions.AsNoTracking()
                        .GroupBy(x => x.CustomerId)
                        .Select(x => x.OrderByDescending(y => y.CreatedDate).FirstOrDefault())
                        .Where(x => x != null && x.MembershipTypeId == premiumPlus);
            
            if (!string.IsNullOrEmpty(request.OutletId))
            {
                query = query.Where(x => x.OutletId == request.OutletId);
            }

            if (request.FromDateFilter.HasValue && request.ToDateFilter.HasValue)
            {
                query = query.Where(y => y.ExpiredDate.HasValue
                && y.ExpiredDate.Value <= request.ToDateFilter.Value
                && y.ExpiredDate.Value >= request.FromDateFilter.Value);
            }

            var queryExpired = query
                .Join(_dbContext.Customers,
                            trans => trans.CustomerId,
                            customers => customers.Id,
                            (trans, customers) => new { Trans = trans, Customer = customers })
                .Select(x => new ExpiredCustomersModel
            {
                Id = x.Customer.Id,
                FirstName = x.Customer.FirstName,
                LastName = x.Customer.LastName,
                Phone = x.Customer.Phone,
                PhoneCountryCode = x.Customer.PhoneCountryCode,
                Email = x.Customer.Email,
                DateOfBirth = x.Customer.DateOfBirth,
                LastUsed = x.Customer.LastUsed,
                JoinedDate = x.Customer.JoinedDate,
                Status = x.Customer.Status.ToString(),
                ExpiredDate = x.Trans != null ? x.Trans.ExpiredDate : null,
                Comment = x.Trans != null ? x.Trans.Comment : "",
                CustomerCode = x.Customer.CustomerCode
            })
            .OrderByDescending(x => x.ExpiredDate)
            .AsQueryable();

            var result = PagingExtensions.GetPaged<ExpiredCustomersModel>(queryExpired, request.PageNumber, request.PageSize);
            var response = new GetExpiredCustomersResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.CustomerListResponse = result.Results;
            return response;
        }
    }
}
