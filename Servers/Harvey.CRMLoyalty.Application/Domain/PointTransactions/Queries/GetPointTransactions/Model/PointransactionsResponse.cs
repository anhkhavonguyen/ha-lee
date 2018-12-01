using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries.Model
{
    public class GetPointTransactionsResponse : BaseResponse
    {
        public List<PointTransactionModel> PointTransactionModels { get; set; }
    }
}
