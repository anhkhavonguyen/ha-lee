using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetTotalBalancePointTransactionQuery : IGetTotalBalancePointTransactionQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetTotalBalancePointTransactionQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetTotalBalancePointTransactionResponse Execute(GetTotalBalancePointTransactionRequest request)
        {
            var queryPointTransaction = _dbContext.PointTransactions.AsNoTracking()
                .Select(o => new PointTransactionModel
                {
                    Id = o.Id,
                    Comment = o.Comment,
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                    OutletName = o.Outlet.Name,
                    StaffName = o.Staff.FirstName + " " + o.Staff.LastName,
                    ExpiredDate = o.ExpiredDate,
                    PhoneCustomer = o.Customer.Phone,
                    PhoneCountryCode = o.Customer.PhoneCountryCode,
                    Debit = o.Debit,
                    Credit = o.Credit,
                    BalanceTotal = o.BalanceTotal,
                    Voided = o.Voided,
                    VoidedBy = o.VoidedBy.FirstName + " " + o.VoidedBy.LastName,
                    CreatedDate = o.CreatedDate,
                    OutletId = o.OutletId,
                    IPAddress = o.IPAddress,
                    CustomerId = o.CustomerId
                })
                .AsQueryable();

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

            var response = new GetTotalBalancePointTransactionResponse();
            response.TotalBalance = queryPointTransaction.Sum(x => x.Credit) - queryPointTransaction.Sum(x => x.Debit);
            return response;
        }
    }
}
