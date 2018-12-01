using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics.Model;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics
{
    public class GetWalletStatisticsQuery : IGetWalletStatisticsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetWalletStatisticsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetWalletStatisticsResponse Execute(GetWalletStatisticsRequest request)
        {
            var result = new GetWalletStatisticsResponse();

            var listDataWalletStatistics = new List<DataWalletStatisticsPerDay>();
            var listDayWalletStatistics = GetListDayInPeriodTime(request.FromDate.Date, request.ToDate.Date);

            foreach (var day in listDayWalletStatistics)
            {
                var dayData = GetDataWalletStatisticsPerDay(day.Date, request.OutletId);
                listDataWalletStatistics.Add(dayData);
            }

            result.DataWalletStatistics = listDataWalletStatistics;

            return result;
        }

        private List<DateTime> GetListDayInPeriodTime(DateTime fromDate, DateTime toDate)
        {
            var listDayInPeriodTime = new List<DateTime>();

            for (var time = fromDate; time <= toDate; time = time.AddDays(1))
            {
                listDayInPeriodTime.Add(time);
            }

            return listDayInPeriodTime;
        }

        private DataWalletStatisticsPerDay GetDataWalletStatisticsPerDay(DateTime day, string outletId)
        {
            var result = new DataWalletStatisticsPerDay();
            result.Time = day;

            var queryTopup = _dbContext.WalletTransactions.Where(a => a.CreatedDate != null
            && a.CreatedDate.Value.Date == day
            && a.Credit != 0);

            var querySpend = _dbContext.WalletTransactions.Where(a => a.CreatedDate != null
            && a.CreatedDate.Value.Date == day
            && a.Debit != 0);

            if (!string.IsNullOrEmpty(outletId))
            {

                result.TotalTopup = Convert.ToDecimal(Math.Round(queryTopup.Where(a => a.OutletId == outletId).Sum(a => a.Credit), 2));
                result.TotalSpend = Convert.ToDecimal(Math.Round(querySpend.Where(a => a.OutletId == outletId).Sum(a => a.Debit), 2));
            } else
            {
                result.TotalTopup = Convert.ToDecimal(Math.Round(queryTopup.Sum(a => a.Credit), 2));
                result.TotalSpend = Convert.ToDecimal(Math.Round(querySpend.Sum(a => a.Debit), 2));
            }

            return result;
        }
    }
}
