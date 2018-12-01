using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class GetNewCustomersQuery : IGetNewCustomersQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetNewCustomersQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetNewCustomersResponse Execute(GetNewCustomersRequest request)
        {
            var query = _dbContext.MembershipTransactions
                       .Where(x => (Data.MembershipActionType)x.MembershipActionTypeId == Data.MembershipActionType.New
                               && x.CreatedDate.Value >= request.FromDateFilter
                               && x.CreatedDate.Value <= request.ToDateFilter);

            if (!string.IsNullOrEmpty(request.OutletId))
            {
                query = query.Where(x => x.OutletId == request.OutletId);
            }

            var queryGetNewCustomers = query
                .Include(x => x.Customer)
                .Select(x => new CustomerModel
            {
                Id = x.Customer.Id,
                FirstName = x.Customer.FirstName,
                LastName = x.Customer.LastName,
                Phone = x.Customer.Phone,
                PhoneCountryCode = x.Customer.PhoneCountryCode,
                Email = x.Customer.Email,
                DateOfBirth = x.Customer.DateOfBirth,
                LastUsed = x.Customer.LastUsed,
                JoinedDate = x.Customer.JoinedDate.Value,
                Status = x.Customer.Status.ToString(),
                Comment = x.Comment,
                CustomerCode = x.Customer.CustomerCode,
                Gender = x.Customer.Gender
            }).OrderByDescending(x => x.JoinedDate.Value)
           .AsQueryable();

            var result = PagingExtensions.GetPaged<CustomerModel>(queryGetNewCustomers, request.PageNumber, request.PageSize);

            var response = new GetNewCustomersResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.CustomerListResponse = result.Results;

            return response;
        }

    }
}
