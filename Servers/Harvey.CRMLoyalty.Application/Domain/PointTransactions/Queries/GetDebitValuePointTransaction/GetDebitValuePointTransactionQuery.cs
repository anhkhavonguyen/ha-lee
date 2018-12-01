using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetDebitValuePointTransactionQuery : IGetDebitValuePointTransactionQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetDebitValuePointTransactionQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetDebitValuePointTransactionResponse Execute(GetDebitValuePointTransactionRequest request)
        {
            var queryPointTransaction = _dbContext.PointTransactions.AsNoTracking()
                .Where(x => x.Credit == 0 
                    && (x.PointTransactionReference == null
                        || (x.PointTransactionReference != null && x.PointTransactionReference.PointTransactionReference != null)))
                .Select(o => new PointTransactionModel
            {
                Id = o.Id,
                Comment = o.Comment,
                CustomerName = o.Customer != null ? o.Customer.FirstName + " " + o.Customer.LastName : "",
                OutletName = o.Outlet != null ? o.Outlet.Name : "",
                StaffName = o.Staff != null ? o.Staff.FirstName + " " + o.Staff.LastName : "",
                ExpiredDate = o.ExpiredDate,
                PhoneCustomer = o.Customer != null ? o.Customer.Phone : "",
                PhoneCountryCode = o.Customer != null ? o.Customer.PhoneCountryCode : "",
                Debit = o.Debit,
                Credit = o.Credit,
                BalanceTotal = o.BalanceTotal,
                Voided = o.Voided,
                VoidedBy = o.VoidedBy != null ? o.VoidedBy.FirstName + " " + o.VoidedBy.LastName : "",
                CreatedDate = o.CreatedDate,
                OutletId = o.OutletId,
                IPAddress = o.IPAddress,
                CustomerId = o.Customer != null ? o.CustomerId : "",
                CustomerCode = o.Customer != null ? o.Customer.CustomerCode : ""
                })
            .OrderByDescending(x => x.CreatedDate).AsQueryable();

            if (!string.IsNullOrEmpty(request.OutletId))
            {
                queryPointTransaction = queryPointTransaction.Where(x => x.OutletId == request.OutletId);
            }

            if (request.FromDateFilter.HasValue && request.ToDateFilter.HasValue)
            {
                queryPointTransaction = queryPointTransaction.Where(y => (y.CreatedDate != null 
                                                                    && y.CreatedDate.Value < request.ToDateFilter.Value
                                                                    && y.CreatedDate.Value > request.FromDateFilter.Value));
            }

            var result = PagingExtensions.GetPaged<PointTransactionModel>(queryPointTransaction, request.PageNumber, request.PageSize);
            var response = new GetDebitValuePointTransactionResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListPointTransaction = result.Results;
            response.TotalDebitValue = queryPointTransaction.Sum(x => x.Debit);
            return response;
        }
    }
}
