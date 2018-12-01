using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetTotalBalanceWalletTransactionQuery : IGetTotalBalanceWalletTransactionQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetTotalBalanceWalletTransactionQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetTotalBalanceWalletTransactionResponse Execute(GetTotalBalanceWalletTransactionRequest request)
        {
            var queryWalletTransaction = _dbContext.WalletTransactions.AsNoTracking()
                .Select(o => new WalletTransactionModel
                {
                    Id = o.Id,
                    Comment = o.Comment,
                    CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                    OutletName = o.Outlet != null ? o.Outlet.Name : "",
                    StaffName = o.Staff != null ? o.Staff.FirstName + " " + o.Staff.LastName : o.CreatedByName,
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
                queryWalletTransaction = queryWalletTransaction.Where(x => x.OutletId == request.OutletId);
            }

            if (request.FromDateFilter.HasValue && request.ToDateFilter.HasValue)
            {
                queryWalletTransaction = queryWalletTransaction.Where(y => (y.CreatedDate != null
                                                                    && y.CreatedDate.Value < request.ToDateFilter.Value
                                                                    && y.CreatedDate.Value > request.FromDateFilter.Value));
            }

            var response = new GetTotalBalanceWalletTransactionResponse();
            response.TotalBalance = queryWalletTransaction.Sum(x => x.Credit) - queryWalletTransaction.Sum(x => x.Debit);
            return response;
        }
    }
}
