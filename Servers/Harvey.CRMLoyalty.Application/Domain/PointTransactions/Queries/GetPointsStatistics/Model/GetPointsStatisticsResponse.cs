using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics.Model
{
    public class GetPointsStatisticsResponse
    {
        public List<DataPointsStatisticsPerDay> DataPointsStatistics { get; set; }
    }

    public class DataPointsStatisticsPerDay
    {
        public DateTime Time { get; set; }
        public decimal TotalAdd { get; set; }
        public decimal TotalRedeem { get; set; }
    }
}
