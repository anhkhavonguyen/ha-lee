using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetDebitValuePointTransactionRequest : BaseRequest
    {
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string OutletId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
