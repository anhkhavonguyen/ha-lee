using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers.Model;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries.GetUpgradedCustomers
{
    public class GetUpgradedCustomersQuery : IGetUpgradedCustomersQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetUpgradedCustomersQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public GetUpgradedCustomersResponse Execute(GetUpgradedCustomersRequest request)
        {
            var query = _dbContext.MembershipTransactions
                        .Where(x => ((Data.MembershipActionType)x.MembershipActionTypeId == Data.MembershipActionType.Upgrade)
                                && x.CreatedDate.Value >= request.FromDateFilter
                                && x.CreatedDate.Value <= request.ToDateFilter)
                        .GroupBy(x => x.CustomerId)
                        .Select(x => x.OrderByDescending(y => y.CreatedDate).FirstOrDefault());

            if (!string.IsNullOrEmpty(request.OutletId))
            {
                query = query.Where(x => x.OutletId == request.OutletId);
            }

            var response = new GetUpgradedCustomersResponse();
            response.PageSize = request.PageSize;
            response.PageNumber = request.PageNumber;
            if (query != null && query.Any())
            {
                var queryCustomer = query
                    .Join(_dbContext.Customers,
                            trans => trans.CustomerId,
                            customers => customers.Id,
                            (trans, customers) => new { Trans = trans, Customer = customers })
                    .Select(x => new UpgradedCustomerModel
                    {
                        Id = x.Customer != null ? x.Customer.Id : "",
                        FirstName = x.Customer != null ? x.Customer.FirstName : "",
                        LastName = x.Customer != null ? x.Customer.LastName : "",
                        Phone = x.Customer != null ? x.Customer.Phone : "",
                        PhoneCountryCode = x.Customer != null ? x.Customer.PhoneCountryCode : "",
                        Email = x.Customer != null ? x.Customer.Email : "",
                        DateOfBirth = x.Customer != null ? x.Customer.DateOfBirth : null,
                        JoinedDate = x.Customer != null ? x.Customer.JoinedDate : null,
                        Status = x.Customer != null ? x.Customer.Status.ToString() : "",
                        Comment = x.Trans.Comment,
                        UpgradedDate = x.Customer != null ? x.Customer.CreatedDate : null,
                        CustomerCode = x.Customer != null ? x.Customer.CustomerCode : ""
                    })
                .OrderByDescending(x => x.JoinedDate)
                .AsQueryable();
                var result = PagingExtensions.GetPaged<UpgradedCustomerModel>(queryCustomer, request.PageNumber, request.PageSize);
                response.TotalItem = result.TotalItem;
                response.CustomerListResponse = result.Results;
            }
            return response;
        }
    }
}
