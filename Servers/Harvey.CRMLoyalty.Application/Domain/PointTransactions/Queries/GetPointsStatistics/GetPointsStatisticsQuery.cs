using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics.Model;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics
{
    public class GetPointsStatisticsQuery : IGetPointsStatisticsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;
        public GetPointsStatisticsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetPointsStatisticsResponse Execute(GetPointsStatisticsRequest request)
        {
            var result = new GetPointsStatisticsResponse();

            var listDataPointsStatistic = new List<DataPointsStatisticsPerDay>();
            var listDayPointsStatistic = GetListDayInPeriodTime(request.FromDate.Date, request.ToDate.Date);

            foreach(var day in listDayPointsStatistic)
            {
                var dayData = GetDataPointsStatisticePerDay(day.Date, request.OutletId);
                listDataPointsStatistic.Add(dayData);
            }

            result.DataPointsStatistics = listDataPointsStatistic;
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

        private DataPointsStatisticsPerDay GetDataPointsStatisticePerDay(DateTime day, string outletId)
        {
            var result = new DataPointsStatisticsPerDay();
            result.Time = day;

            var queryAdd = _dbContext.PointTransactions.Where(a => a.CreatedDate != null
            && a.Credit != 0
            && a.CreatedDate.Value.Date == day);

            var queryRedeem = _dbContext.PointTransactions.Where(a => a.CreatedDate != null
            && a.Debit != 0
            && a.CreatedDate.Value.Date == day);

            if (!string.IsNullOrEmpty(outletId))
            {
                result.TotalAdd = Convert.ToDecimal(Math.Round(queryAdd.Where(a => a.OutletId == outletId).Sum(a => a.Credit), 2));
                result.TotalRedeem = Convert.ToDecimal(Math.Round(queryRedeem.Where(a => a.OutletId == outletId).Sum(a => a.Debit), 2));
            } else
            {
                result.TotalAdd = Convert.ToDecimal(Math.Round(queryAdd.Sum(a => a.Credit), 2));
                result.TotalRedeem = Convert.ToDecimal(Math.Round(queryRedeem.Sum(a => a.Debit), 2));
            }

            return result;
        }
    }
}
