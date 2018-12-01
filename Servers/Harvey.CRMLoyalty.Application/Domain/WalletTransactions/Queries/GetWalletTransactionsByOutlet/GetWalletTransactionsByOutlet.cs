using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet.Model;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet
{
    public class GetWalletTransactionsByOutlet : IGetWalletTransactionsByOutlet
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetWalletTransactionsByOutlet(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetWalletTransactionsByOutletResponse Execute(GetWalletTransactionsByOutletRequest request)
        {
            var query = _dbContext.WalletTransactions.AsNoTracking()
                .Where(x => x.OutletId == request.OutletId && x.WalletTransactionReferenceId == null)
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
                    VoidedBy = _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault() != null
                                    ? _dbContext.WalletTransactions.AsNoTracking()
                                    .Where(x => x.WalletTransactionReferenceId == o.Id)
                                    .FirstOrDefault().CreatedByName
                                    : "",
                    CreatedDate = o.CreatedDate,
                    CustomerCode = o.Customer.CustomerCode
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();
            var result = PagingExtensions.GetPaged<WalletTransactionModel>(query, request.PageNumber, request.PageSize);
            var response = new GetWalletTransactionsByOutletResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListWalletTransaction = result.Results;
            return response;
        }
    }
}
