using AutoMapper.QueryableExtensions;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class GetCustomersQuery : IGetCustomersQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetCustomersQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomersResponse Execute(CustomersRequest request)
        {
            var query = _dbContext.Customers.AsNoTracking().Select(x => new
            {
                customer = x,
                firstMembership = x.MembershipTransactions.OrderBy(y => y.CreatedDate).FirstOrDefault()
            });

            if (!string.IsNullOrEmpty(request.OutletId))
            {
                query = query.Where(x => x.firstMembership != null && x.firstMembership.OutletId == request.OutletId);
            }       
            if (request.FromDateFilter.HasValue && request.ToDateFilter.HasValue)
            {
                query = query.Where(x => x.customer.JoinedDate != null 
                && x.customer.JoinedDate.Value < request.ToDateFilter.Value
                && x.customer.JoinedDate.Value > request.FromDateFilter.Value);
            }
            var queryCustomer = query.Select(x => x.customer);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var searchString = request.SearchText.ToLower().Trim();
                queryCustomer = queryCustomer.Where(x => (x.FirstName != null && x.FirstName.ToLower().Trim().Contains(searchString))
                                    || (x.LastName != null && x.LastName.ToLower().Trim().Contains(searchString))
                                    || (x.Email != null && x.Email.ToLower().Trim().Contains(searchString))
                                    || (x.FirstName != null && x.LastName != null&& ($"{x.FirstName} {x.LastName}").ToLower().Trim().Contains(searchString))
                                    || (x.PhoneCountryCode != null && x.Phone != null && ($"+{x.PhoneCountryCode} {x.Phone}").Contains(searchString)));
            }
            var result = PagingExtensions.GetPaged<Customer, CustomerModel>(queryCustomer, request.PageNumber, request.PageSize);
            var response = new CustomersResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.CustomerListResponse = result.Results;
            return response;
        }

        public CustomersResponse GetCustomersbyCustomerCodes(CustomersRequest request)
        {
            var response = new CustomersResponse();
            var customerCodes = request.CustomerCodes;
            if (customerCodes == null || !customerCodes.Any())
            {
                response.CustomerListResponse = new List<CustomerModel>();
                return response;
            }

            var queryCustomers = _dbContext.Customers.AsNoTracking().Where(x => customerCodes.Contains(x.CustomerCode));
            response.CustomerListResponse = queryCustomers.ProjectTo<CustomerModel>().ToList();

            return response;
        }
    }
}
