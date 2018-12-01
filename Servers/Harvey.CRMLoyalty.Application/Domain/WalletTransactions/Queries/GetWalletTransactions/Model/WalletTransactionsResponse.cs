using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.Model
{
    public class GetWalletTransactionsResponse : BaseResponse
    {
        public List<WalletTransactionModel> WalletTransactionModels { get; set; }
    }
}
