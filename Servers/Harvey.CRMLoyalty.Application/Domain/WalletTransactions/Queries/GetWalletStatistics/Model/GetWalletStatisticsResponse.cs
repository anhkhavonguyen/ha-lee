using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletStatistics.Model
{
    public class GetWalletStatisticsResponse
    {
        public List<DataWalletStatisticsPerDay> DataWalletStatistics { get; set; }
    }

    public class DataWalletStatisticsPerDay
    {
        public DateTime Time { get; set; }
        public decimal TotalTopup { get; set; }
        public decimal TotalSpend { get; set; }
    }
}
