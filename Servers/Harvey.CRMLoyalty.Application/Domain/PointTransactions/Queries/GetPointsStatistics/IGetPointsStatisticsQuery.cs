using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetPointsStatistics
{
    public interface IGetPointsStatisticsQuery
    {
        GetPointsStatisticsResponse Execute(GetPointsStatisticsRequest request);
    }
}
