using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetDebitValuePointTransactionResponse : BaseResponse
    {
        public List<PointTransactionModel> ListPointTransaction { get; set; }

        public decimal TotalDebitValue { get; set; }
    }
}
