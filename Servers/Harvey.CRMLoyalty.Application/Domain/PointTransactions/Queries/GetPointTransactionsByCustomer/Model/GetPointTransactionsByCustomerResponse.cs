using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public class GetPointTransactionsByCustomerResponse: BaseResponse
    {
        public List<PointTransactionModel> ListPointTransaction { get; set; }
    }
}
