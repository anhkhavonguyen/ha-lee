using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System.Collections.Generic;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetWalletTransactionsByCustomerResponse: BaseResponse
    {
        public List<WalletTransactionModel> ListWalletTransaction { get; set; }
    }
}
