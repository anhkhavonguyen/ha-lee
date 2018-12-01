using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetWalletTransactionsByCustomerQuery : IGetWalletTransactionsByCustomerQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetWalletTransactionsByCustomerQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetWalletTransactionsByCustomerResponse Execute(GetWalletTransactionsByCustomerRequest request)
        {
            var query = _dbContext.WalletTransactions.AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId)
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
                    Voided = _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault() != null 
                            ? true 
                            : false,
                    CreatedDate = o.CreatedDate,
                    IPAddress = o.IPAddress,
                    VoidedBy = _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault() != null
                                    ? _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault().CreatedByName
                                    : "",
                    WalletTransactionReferenceId = o.WalletTransactionReferenceId
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();
            var result = PagingExtensions.GetPaged<WalletTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetWalletTransactionsByCustomerResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListWalletTransaction = result.Results;
            return response;
        }

        public GetWalletTransactionsByCustomerResponse GetByMember(GetWalletTransactionsByCustomerRequest request)
        {
            var query = _dbContext.WalletTransactions.AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId && x.WalletTransactionReferenceId == null)
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
                    Voided = _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault() != null
                            ? true
                            : false,
                    CreatedDate = o.CreatedDate,
                    IPAddress = o.IPAddress,
                    VoidedBy = _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault() != null
                                    ? _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault().CreatedByName
                                    : "",
                    WalletTransactionReferenceId = o.WalletTransactionReferenceId
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();
            var result = PagingExtensions.GetPaged<WalletTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetWalletTransactionsByCustomerResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListWalletTransaction = result.Results;
            return response;
        }
    }
}
