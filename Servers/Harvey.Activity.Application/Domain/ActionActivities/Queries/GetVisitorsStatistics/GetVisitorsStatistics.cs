using Harvey.Activity.Api;
using Harvey.Activity.Application.Data;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime
{
    public class GetVisitorsStatistics : IGetVisitorsStatistics
    {
        private readonly HarveyActivityDbContext _dbContext;
        public GetVisitorsStatistics(HarveyActivityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetVisitorsStatisticsResponse Execute(GetVisitorsStatisticsRequest request)
        {
            var result = new GetVisitorsStatisticsResponse();
            var listDataVisitorsStatistics = new List<DataVisitorsStatisticsPerDay>();
            var listDayInPeriodTime = GetListDayInPeriodTime(request.FromDate.Date, request.ToDate.Date);

            foreach (var day in listDayInPeriodTime)
            {
                var dayData = GetDataVisitorsStatisticsPerDay(day.Date, request.OutletId);
                listDataVisitorsStatistics.Add(dayData);
            }
            result.DataVisitorsStatistic = listDataVisitorsStatistics;
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
        private DataVisitorsStatisticsPerDay GetDataVisitorsStatisticsPerDay(DateTime day, string outletId)
        {
            var result = new DataVisitorsStatisticsPerDay();

            var loginServingCustomerTypeAction = ((int)ActionType.LoginServingCustomer).ToString();
            var initCustomerTypeAction = ((int)ActionType.InitCustomer).ToString();

            var queryFirstTimeVisitor = _dbContext.Activities.Where(a => a.CreatedDate.Value.Date == day
            && a.ActionTypeId == initCustomerTypeAction);

            var queryUniqueVisitor = _dbContext.Activities.Where(a => a.CreatedDate.Value.Date == day
            && a.ActionTypeId == loginServingCustomerTypeAction);

            result.Time = day;

            if (!string.IsNullOrEmpty(outletId))
            {
                result.Value = queryFirstTimeVisitor.Where(a => a.Value == outletId).ToList().Count();
                result.UniqueValue = queryUniqueVisitor.Where(a => a.Value == outletId).GroupBy(a => a.Description).ToList().Count();
            }
            else
            {
                result.Value = queryFirstTimeVisitor.ToList().Count();
                result.UniqueValue = queryUniqueVisitor.GroupBy(a => a.Description).ToList().Count();
            }

            return result;
        }
    }
}
