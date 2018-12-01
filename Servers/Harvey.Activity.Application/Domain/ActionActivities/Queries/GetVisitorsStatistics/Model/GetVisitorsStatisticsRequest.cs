using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime.Model
{
    public class GetVisitorsStatisticsRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string OutletId { get; set; }
    }
}
