using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints.Model;
using Harvey.CRMLoyalty.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.GetExpiringPoints
{
    public class GetExpiryPointsQuery : IGetExpiryPointsQuery
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public GetExpiryPointsQuery(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetExpiryPointsResponse> GetExpiryPointAsync(GetExpiryPointsRequest request)
        {
            var lastTransaction = await GetLastTransactionAddAsync(request.CustomerId);
            decimal availablePoint = await GetAvailablePointAsync(request.CustomerId);
            decimal expiringPoint = 0;
            var expiryPoints = new List<ExpiryPoint>();
            var groupExpiryPoint = new GroupExpiryPoint();

            if (availablePoint == 0)
            {
                expiringPoint = 0;
                expiryPoints = new List<ExpiryPoint>();
            } else
            {
                if (lastTransaction != null)
                {
                    if (lastTransaction.ExpiredDate.Value.Date <= request.ToDate.Date)
                    {
                        groupExpiryPoint = GetExpiryPointsWithLastItemInPeriodTime(availablePoint, request);
                        expiringPoint = groupExpiryPoint.TotalExpirypoint;
                        expiryPoints = groupExpiryPoint.expiryPoints;
                    }
                    else
                    {
                        groupExpiryPoint = GetExpiryPointsWithLastItemOutPeriodTime(availablePoint, request);
                        expiringPoint = groupExpiryPoint.TotalExpirypoint;
                        expiryPoints = groupExpiryPoint.expiryPoints;
                    }
                }
                else
                {
                    expiringPoint = 0;
                    expiryPoints = new List<ExpiryPoint>();
                }
            }

            var response = new GetExpiryPointsResponse
            {
                TotalAvailablePoint = availablePoint,
                TotalExpirypoint = expiringPoint,
                ExpiryPoints = groupExpiryPoint.expiryPoints
            };

            return response;
        }

        public GroupExpiryPoint GetExpiryPointsWithLastItemInPeriodTime(decimal availablePoint, GetExpiryPointsRequest request)
        {
            var result = new GroupExpiryPoint();
            var expiryPoints = new List<ExpiryPoint>();

            decimal expiringPoint = 0;
            decimal tempAvailablePoints = availablePoint;

            var expiryTransactions = _dbContext.PointTransactions.AsNoTracking()
                       .Where(x => x.ExpiredDate.Value.Date <= request.ToDate.Date
                       && x.ExpiredDate.Value.Date >= request.FromDate.Date
                       && x.CustomerId == request.CustomerId
                       && x.Credit != 0
                       && x.PointTransactionReferenceId == null
                       && !_dbContext.PointTransactions.Any(y => y.PointTransactionReferenceId == x.Id))
                       .GroupBy(o => o.ExpiredDate.Value.Date)
                       .Select(x => new
                       {
                           expiry = x.Key,
                           pointTransactions = x
                       })
                       .ToList();

            for (int i = expiryTransactions.Count - 1; i >= 0; i--)
            {
                if (tempAvailablePoints <= 0) { break; }
                var temp = new ExpiryPoint();
                temp.Expiry = expiryTransactions[i].expiry.Date;
                foreach (var item in expiryTransactions[i].pointTransactions)
                {
                    if (tempAvailablePoints - item.Credit >= 0)
                    {
                        temp.PointValue += item.Credit;
                        expiringPoint += item.Credit;
                        tempAvailablePoints -= item.Credit;
                    }
                    else
                    {
                        temp.PointValue += tempAvailablePoints;
                        expiringPoint += tempAvailablePoints;
                        tempAvailablePoints -= item.Credit;
                        break;
                    }
                }
                expiryPoints.Add(temp);
            }

            result.TotalExpirypoint = expiringPoint;
            result.expiryPoints = expiryPoints;

            return result;
        }

        public GroupExpiryPoint GetExpiryPointsWithLastItemOutPeriodTime(decimal availablePoint, GetExpiryPointsRequest request)
        {
            var result = new GroupExpiryPoint();

            decimal tempAvailablePoints = availablePoint;
            decimal expiringPoint = 0;
            var expiryPoints = new List<ExpiryPoint>();

            var expiryTransactionsOutOfPeriodTime = _dbContext.PointTransactions.AsNoTracking()
                .Where(x => x.ExpiredDate.Value.Date > request.ToDate.Date
                && x.CustomerId == request.CustomerId
                && x.Credit != 0
                && x.PointTransactionReferenceId == null
                && !_dbContext.PointTransactions.Any(y => y.PointTransactionReferenceId == x.Id))
                .GroupBy(o => o.ExpiredDate.Value.Date)
                .Select(x => new
                {
                    expiry = x.Key,
                    pointTransactions = x
                })
                .ToList();

            for (int i = expiryTransactionsOutOfPeriodTime.Count - 1; i >= 0; i--)
            {
                if (tempAvailablePoints <= 0)
                {
                    expiringPoint = 0;
                    expiryPoints = new List<ExpiryPoint>();
                    break;
                }
                foreach (var item in expiryTransactionsOutOfPeriodTime[i].pointTransactions)
                {
                    tempAvailablePoints -= item.Credit;
                }
            }

            if (tempAvailablePoints > 0)
            {
                result = GetExpiryPointsWithLastItemInPeriodTime(tempAvailablePoints, request);
            } else
            {
                result.TotalExpirypoint = expiringPoint;
                result.expiryPoints = expiryPoints;
            }

            return result;
        }

        public async Task<PointTransaction> GetLastTransactionAddAsync(string customerId)
        {
            var customerLastedTransaction = await _dbContext.PointTransactions.AsNoTracking().Where(a => a.CustomerId == customerId && a.Credit != 0 && a.ExpiredDate != null).OrderByDescending(a => a.CreatedDate).FirstOrDefaultAsync();
            return customerLastedTransaction;
        }

        public async Task<decimal> GetAvailablePointAsync(string customerId)
        {
            var customerLastedTransaction = await _dbContext.PointTransactions.AsNoTracking().Where(a => a.CustomerId == customerId).OrderByDescending(a => a.CreatedDate).FirstOrDefaultAsync();
            return customerLastedTransaction != null ? customerLastedTransaction.BalanceTotal : 0;
        }
    }
}
