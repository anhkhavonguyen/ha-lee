using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics
{
    public interface IGetWalletStatisticsQuery
    {
        GetWalletStatisticsResponse Execute(GetWalletStatisticsRequest request);
    }
}
