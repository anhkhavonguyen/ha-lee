using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetTotalBalancePointTransactionResponse : BaseResponse
    {
        public decimal TotalBalance { get; set; }
    }
}
