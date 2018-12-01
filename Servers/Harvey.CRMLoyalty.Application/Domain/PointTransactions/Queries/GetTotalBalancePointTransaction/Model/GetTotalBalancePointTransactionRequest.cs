using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetTotalBalancePointTransactionRequest: BaseRequest
    {
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string OutletId { get; set; }
    }
}
