using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime
{
    public interface IGetVisitorsStatistics
    {
        GetVisitorsStatisticsResponse Execute(GetVisitorsStatisticsRequest request);
    }
}
