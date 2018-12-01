using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.Model;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetPointTransactionsQuery : IGetPointTransactionsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetPointTransactionsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetPointTransactionsResponse GetPointTransactions(GetPointTransactionsRequest request)
        {
            var query = _dbContext.PointTransactions.AsNoTracking().AsQueryable();
            var result = PagingExtensions.GetPaged<PointTransaction, PointTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetPointTransactionsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.PointTransactionModels = result.Results;
            return response;
        }

        public GetPointTransactionsResponse GetPointTransactionsByStaff(GetPointTransactionsRequest request)
        {
            var query = _dbContext.PointTransactions.AsNoTracking().Where(a => a.StaffId == request.UserId).AsQueryable();
            var result = PagingExtensions.GetPaged<PointTransaction, PointTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetPointTransactionsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.PointTransactionModels = result.Results;
            return response;
        }

        public decimal GetPointBalance(string customerId)
        {
            var customerLastedTransaction = _dbContext.PointTransactions.AsNoTracking().Where(a => a.CustomerId == customerId).OrderByDescending(a => a.CreatedDate)?.FirstOrDefault();
            return customerLastedTransaction != null ? customerLastedTransaction.BalanceTotal : 0 ;
        }

        public GetPointTransactionsResponse GetPointTransactionsByOutlet(GetPointTransactionsByOutletRequest request)
        {
            var query = _dbContext.PointTransactions.AsNoTracking()
                .Where(x => x.OutletId == request.OutletId && x.PointTransactionReferenceId == null)
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
                    CreatedDate = o.CreatedDate,
                    Voided = _dbContext.PointTransactions.Where(x => x.PointTransactionReferenceId == o.Id).FirstOrDefault() != null ? true : false,
                    VoidedBy = _dbContext.PointTransactions
                                .Where(x => x.PointTransactionReferenceId == o.Id)
                                .FirstOrDefault() != null
                                ? _dbContext.PointTransactions
                                .Where(x => x.PointTransactionReferenceId == o.Id)
                                .FirstOrDefault().CreatedByName
                                : "",
                    CustomerCode = o.Customer.CustomerCode
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();

            var result = PagingExtensions.GetPaged<PointTransactionModel>(query, request.PageNumber, request.PageSize);

            var response = new GetPointTransactionsResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.PointTransactionModels = result.Results;
            return response;
        }
    }
}
