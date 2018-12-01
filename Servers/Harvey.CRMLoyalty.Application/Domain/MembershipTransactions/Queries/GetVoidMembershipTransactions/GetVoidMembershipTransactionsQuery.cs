using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions.Model;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.MembershipTransactions.Queries.GetVoidMembershipTransactions
{
    public class GetVoidMembershipTransactionsQuery : IGetVoidMembershipTransactionsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetVoidMembershipTransactionsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetVoidMembershipTransactionsResponse Execute(GetVoidMembershipTransactionsRequest request)
        {
            var response = new GetVoidMembershipTransactionsResponse();
            if (request == null)
                return response;
            var query = _dbContext.MembershipTransactions.AsNoTracking()
                .Where(x => x.CreatedDate.HasValue
                        && x.CreatedDate >= request.FromDateFilter
                        && x.CreatedDate <= request.ToDateFilter
                        && x.MembershipTransactionReferenceId != null
                        && x.MembershipTransactionReference != null);
            if (!string.IsNullOrEmpty(request.OutletId))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.OutletId) && x.OutletId == request.OutletId);
            }

            if (query != null)
            {
                var queryMembershipModel = query.Include(x => x.MembershipTransactionReference).Select(o => new MembershipTransactionModel
                {
                    Id = o.MembershipTransactionReferenceId,
                    MembershipType = o.MembershipTransactionReference.MembershipType != null ? o.MembershipTransactionReference.MembershipType.TypeName : "",
                    Comment = o.MembershipTransactionReference.Comment,
                    CustomerName = o.MembershipTransactionReference.Customer != null ? o.MembershipTransactionReference.Customer.FirstName + " " + o.MembershipTransactionReference.Customer.LastName : "",
                    OutletName = o.MembershipTransactionReference.Outlet != null ? o.MembershipTransactionReference.Outlet.Name : "",
                    ExpiredDate = o.MembershipTransactionReference.ExpiredDate,
                    PhoneCustomer = o.MembershipTransactionReference.Customer != null ? o.MembershipTransactionReference.Customer.Phone : "",
                    PhoneCountryCode = o.MembershipTransactionReference.Customer != null ? o.MembershipTransactionReference.Customer.PhoneCountryCode : "",
                    CreatedDate = o.MembershipTransactionReference.CreatedDate,
                    VoidedDate = o.CreatedDate,
                    CustomerCode = o.MembershipTransactionReference.Customer != null ? o.MembershipTransactionReference.Customer.CustomerCode : "",
                    VoidedBy = o.CreatedByName,
                    DoneBy = o.MembershipTransactionReference.Staff != null ? o.MembershipTransactionReference.Staff.FirstName + " " + o.MembershipTransactionReference.Staff.LastName : "",
                    IPAddress = o.MembershipTransactionReference.IPAddress,
                    CustomerId = o.CustomerId,

                });
                var result = PagingExtensions.GetPaged<MembershipTransactionModel>(queryMembershipModel, request.PageNumber, request.PageSize);
                response.TotalItem = result.TotalItem;
                response.PageSize = result.PageSize;
                response.PageNumber = result.PageNumber;
                response.ListMembershipTransaction = result.Results;
            }
            return response;
        }
    }
}
