using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetPointTransactionsByCustomerQuery : IGetPointTransactionsByCustomerQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetPointTransactionsByCustomerQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetPointTransactionsByCustomerResponse Execute(GetPointTransactionsByCustomerRequest request)
        {
            const string SystemMigrationData = "System Migration";
            var migrationPointTransaction = _dbContext.PointTransactions.Where(x => x.CustomerId == request.CustomerId).OrderBy(x => x.CreatedDate).FirstOrDefault(x => x.IPAddress == SystemMigrationData);
            var query = _dbContext.PointTransactions.AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId)
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
                    Voided = _dbContext.PointTransactions.AsNoTracking().Where(x => x.PointTransactionReferenceId == o.Id).FirstOrDefault() != null ? true : false,
                    VoidedBy = _dbContext.PointTransactions.AsNoTracking()
                                .Where(x => x.PointTransactionReferenceId == o.Id)
                                .FirstOrDefault() != null
                                ? _dbContext.PointTransactions.AsNoTracking()
                                .Where(x => x.PointTransactionReferenceId == o.Id)
                                .FirstOrDefault().CreatedByName
                                : "",
                    CreatedDate = o.CreatedDate,
                    IPAddress = o.IPAddress,
                    PointTransactionReferenceId = o.PointTransactionReferenceId,
                    AllowVoid = (migrationPointTransaction != null && o.Id == migrationPointTransaction.Id)
                                    || o.PointTransactionReferenceId != null
                                    || (o.ExpiredDate.HasValue && o.ExpiredDate.Value.Date < DateTime.UtcNow.Date)
                                ? false 
                                : true
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();

            var result = PagingExtensions.GetPaged<PointTransactionModel>(query, request.PageNumber, request.PageSize);

            var response = new GetPointTransactionsByCustomerResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListPointTransaction = result.Results;
            return response;
        }

        public GetPointTransactionsByCustomerResponse GetByMember(GetPointTransactionsByCustomerRequest request)
        {
            var query = _dbContext.PointTransactions.AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId && x.Voided != true)
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
                    Voided = _dbContext.PointTransactions.AsNoTracking().Where(x => x.PointTransactionReferenceId == o.Id).FirstOrDefault() != null ? true : false,
                    VoidedBy = _dbContext.PointTransactions.AsNoTracking()
                                .Where(x => x.PointTransactionReferenceId == o.Id)
                                .FirstOrDefault() != null
                                ? _dbContext.PointTransactions.AsNoTracking()
                                .Where(x => x.PointTransactionReferenceId == o.Id)
                                .FirstOrDefault().CreatedByName
                                : "",
                    CreatedDate = o.CreatedDate,
                    IPAddress = o.IPAddress,
                    PointTransactionReferenceId = o.PointTransactionReferenceId
                })
                .OrderByDescending(x => x.CreatedDate).AsQueryable();

            var result = PagingExtensions.GetPaged<PointTransactionModel>(query, request.PageNumber, request.PageSize);

            var response = new GetPointTransactionsByCustomerResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListPointTransaction = result.Results;
            return response;
        }
    }
}
