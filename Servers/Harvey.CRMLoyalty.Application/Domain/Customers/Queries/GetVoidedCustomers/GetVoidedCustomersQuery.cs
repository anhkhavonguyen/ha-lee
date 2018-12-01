using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class GetVoidedCustomersQuery : IGetVoidedCustomersQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetVoidedCustomersQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetVoidedCustomersResponse Execute(GetVoidedCustomersRequest request)
        {
            var query = _dbContext.MembershipTransactions
                        .Where(x => (Data.MembershipActionType)x.MembershipActionTypeId == Data.MembershipActionType.Void
                                && x.CreatedDate.Value >= request.FromDateFilter
                                && x.CreatedDate.Value <= request.ToDateFilter)
                        .GroupBy(x => x.CustomerId)
                        .Select(x => x.OrderByDescending(y => y.CreatedDate).FirstOrDefault());
            if (!string.IsNullOrEmpty(request.OutletId))
            {
                query = query.Where(x => x.OutletId == request.OutletId);
            }

            var response = new GetVoidedCustomersResponse();
            response.PageSize = request.PageSize;
            response.PageNumber = request.PageNumber;
            if (query != null && query.Any())
            {
                var queryDowngraded = query
                    .Join(_dbContext.Customers, 
                            trans => trans.CustomerId, 
                            customers => customers.Id, 
                            (trans, customers) => new {Trans = trans, Customer = customers})
                    .Select(x => new VoidedCustomersModel
                {
                    Id = x.Customer != null ? x.Customer.Id : "",
                    FirstName = x.Customer != null ? x.Customer.FirstName : "",
                    LastName = x.Customer != null ? x.Customer.LastName : "",
                    Phone = x.Customer != null ? x.Customer.Phone : "",
                    PhoneCountryCode = x.Customer != null ? x.Customer.PhoneCountryCode : "",
                    Email = x.Customer != null ? x.Customer.Email : "",
                    DateOfBirth = x.Customer != null ? x.Customer.DateOfBirth : null,
                    LastUsed = x.Customer != null ? x.Customer.LastUsed : null,
                    JoinedDate = x.Customer != null ? x.Customer.JoinedDate : null,
                    Status = x.Customer != null ? x.Customer.Status.ToString() : "",
                    VoidedDate = x.Trans.CreatedDate,
                    Comment = x.Trans.Comment,
                    CustomerCode = x.Customer != null ? x.Customer.CustomerCode : ""
                })
            .OrderByDescending(x => x.VoidedDate)
            .AsQueryable();

                var result = PagingExtensions.GetPaged<VoidedCustomersModel>(queryDowngraded, request.PageNumber, request.PageSize);
                response.TotalItem = result.TotalItem;
                response.CustomerListResponse = result.Results;
            }
            return response;
        }
    }
}
