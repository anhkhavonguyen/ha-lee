using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetDebitValueWalletTransactionQuery : IGetDebitValueWalletTransactionQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetDebitValueWalletTransactionQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetDebitValueWalletTransactionResponse Execute(GetDebitValueWalletTransactionRequest request)
        {
            var queryWalletTransaction = _dbContext.WalletTransactions.AsNoTracking()
                .Where(x => x.Credit == 0
                && (x.WalletTransactionReference == null
                        || (x.WalletTransactionReference != null && x.WalletTransactionReference.WalletTransactionReference != null)))
                .Select(o => new WalletTransactionModel
            {
                Id = o.Id,
                Comment = o.Comment,
                CustomerName = o.Customer != null ? o.Customer.FirstName + " " + o.Customer.LastName : "",
                OutletName = o.Outlet != null ? o.Outlet.Name : "",
                StaffName = o.Staff != null ? o.Staff.FirstName + " " + o.Staff.LastName : o.CreatedByName,
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
                queryWalletTransaction = queryWalletTransaction.Where(x => x.OutletId == request.OutletId);
            }

            if (request.FromDateFilter.HasValue && request.ToDateFilter.HasValue)
            {
                queryWalletTransaction = queryWalletTransaction.Where(y => (y.CreatedDate != null
                                                                    && y.CreatedDate.Value < request.ToDateFilter.Value
                                                                    && y.CreatedDate.Value > request.FromDateFilter.Value));
            }

            var result = PagingExtensions.GetPaged<WalletTransactionModel>(queryWalletTransaction, request.PageNumber, request.PageSize);
            var response = new GetDebitValueWalletTransactionResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListWalletTransaction = result.Results;
            response.TotalDebitValue = queryWalletTransaction.Sum(x => x.Debit);
            return response;
        }
    }
}
