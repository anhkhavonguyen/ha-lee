using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime.Model
{
    public class GetVisitorsStatisticsResponse
    {
        public List<DataVisitorsStatisticsPerDay> DataVisitorsStatistic { get; set; }
    }

    public class DataVisitorsStatisticsPerDay
    {
        public DateTime Time { get; set; }
        public decimal Value { get; set; }
        public decimal UniqueValue { get; set; }
    }
}
