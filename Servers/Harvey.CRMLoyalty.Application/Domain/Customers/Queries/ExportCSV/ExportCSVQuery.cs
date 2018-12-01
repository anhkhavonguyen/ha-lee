using AutoMapper.QueryableExtensions;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.Customers.Queries
{
    public class ExportCSVQuery : IExportCSVQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public ExportCSVQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public byte[] Excute()
        {
            var query = _dbContext
                .Customers
                .Include(x => x.PointTransactions)
                .Include(x => x.WalletTransactions)
                .Select(x => new
                {
                    customer = x,
                    lastPointTrans = x.PointTransactions.OrderByDescending(y => y.CreatedDate).FirstOrDefault(),
                    lastWalletTrans = x.WalletTransactions.OrderByDescending(y => y.CreatedDate).FirstOrDefault()
                })
                .AsNoTracking()
                .AsEnumerable();
            var comlumHeadrs = new string[]
           {
                "First Name",
                "Last Name",
                "Email",
                "Mobile No.",
                "Joined Date",
                "Last Used Date",
                "Status",
                "Date Of Birth",
                "Balance Point",
                "Balance Wallet"
           };

            var customerRecords = (from queryCustomer in query
                                   select new object[]
                                   {
                                        !string.IsNullOrEmpty(queryCustomer.customer.FirstName) ? $"{queryCustomer.customer.FirstName}" : "-",
                                        !string.IsNullOrEmpty(queryCustomer.customer.LastName) ? $"{queryCustomer.customer.LastName}" : "-",
                                        !string.IsNullOrEmpty(queryCustomer.customer.Email) ? $"{queryCustomer.customer.Email}" : "-",
                                        !string.IsNullOrEmpty(queryCustomer.customer.Phone) ? $"+{queryCustomer.customer.PhoneCountryCode} {queryCustomer.customer.Phone}" : "-",
                                        queryCustomer.customer.JoinedDate != null ? $"{queryCustomer.customer.JoinedDate.Value.ToString("MM/dd/yyyy HH:mm")}" : "-",
                                        queryCustomer.customer.LastUsed != null ? $"{queryCustomer.customer.LastUsed.Value.ToString("MM/dd/yyyy HH:mm")}" : "-",
                                        !string.IsNullOrEmpty(queryCustomer.customer.Status.ToString()) ? $"{queryCustomer.customer.Status.ToString()}" : "-",
                                        queryCustomer.customer.DateOfBirth != null ? $"{queryCustomer.customer.DateOfBirth.Value.ToString("MM/dd/yyyy")}" : "-",
                                        queryCustomer.lastPointTrans != null ? queryCustomer.lastPointTrans.BalanceTotal : 0,
                                        queryCustomer.lastWalletTrans != null ? queryCustomer.lastWalletTrans.BalanceTotal : 0
                                   }).ToList();
            var customersCSV = new StringBuilder();
            customerRecords.ForEach(line =>
            {
                customersCSV.AppendLine(string.Join(",", line));
            });
            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{customersCSV.ToString()}");
            return buffer;
        }
    }
}
