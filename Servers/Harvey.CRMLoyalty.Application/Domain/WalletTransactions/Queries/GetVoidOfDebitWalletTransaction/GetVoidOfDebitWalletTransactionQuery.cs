using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetVoidOfDebitWalletTransactionQuery : IGetVoidOfDebitWalletTransactionQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetVoidOfDebitWalletTransactionQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetVoidOfDebitWalletTransactionResponse Execute(GetVoidOfDebitWalletTransactionRequest request)
        {
            var queryWalletTransaction = _dbContext.WalletTransactions
                .Where(x => x.Debit == 0
                && (x.WalletTransactionReference != null && x.WalletTransactionReference.WalletTransactionReference == null))
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
            var response = new GetVoidOfDebitWalletTransactionResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListWalletTransaction = result.Results.ToList();
            response.TotalVoidOfDebitValue = queryWalletTransaction.Sum(x => x.Credit);
            return response;
        }
    }
}
