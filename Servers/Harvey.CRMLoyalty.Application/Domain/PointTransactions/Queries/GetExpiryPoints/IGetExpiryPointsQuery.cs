using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints
{
    public interface IGetExpiryPointsQuery
    {
        Task<GetExpiryPointsResponse> GetExpiryPointAsync(GetExpiryPointsRequest request);
    }
}
