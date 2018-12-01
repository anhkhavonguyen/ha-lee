using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.Model;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetWalletTransactionsQuery : IGetWalletTransactionsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetWalletTransactionsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetWalletTransactionsResponse GetWalletTransactions(GetWalletTransactionsRequest request)
        {
            var result = PagingExtensions.GetPaged<WalletTransaction, WalletTransactionModel>(_dbContext.WalletTransactions.AsNoTracking(), request.PageNumber, request.PageSize);
            var response = new GetWalletTransactionsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.WalletTransactionModels = result.Results;
            return response;
        }

        public GetWalletTransactionsResponse GetWalletTransactionsByStaff(GetWalletTransactionsRequest request)
        {
            var query = _dbContext.WalletTransactions.AsNoTracking().Where(a => a.StaffId == request.UserId);
            var result = PagingExtensions.GetPaged<WalletTransaction, WalletTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetWalletTransactionsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.WalletTransactionModels = result.Results;
            return response;
        }

        public decimal GetWalletBalance(string customerId)
        {
            var customerLastedTransaction = _dbContext.WalletTransactions.AsNoTracking().Where(a => a.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return customerLastedTransaction != null ? customerLastedTransaction.BalanceTotal : 0;
        }
    }
}
