using Harvey.CRMLoyalty.Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints.Model
{
    public class GetExpiryPointsResponse
    {
        public decimal TotalAvailablePoint { get; set; }
        public decimal TotalExpirypoint { get; set; }
        public List<ExpiryPoint> ExpiryPoints { get; set; }
    }

    public class ExpiryPoint
    {
        public DateTime? Expiry { get; set; }
        public decimal PointValue { get; set; }
    }

    public class GroupExpiryPoint
    {
        public decimal TotalExpirypoint { get; set; }
        public List<ExpiryPoint> expiryPoints { get; set; }
    }
}
