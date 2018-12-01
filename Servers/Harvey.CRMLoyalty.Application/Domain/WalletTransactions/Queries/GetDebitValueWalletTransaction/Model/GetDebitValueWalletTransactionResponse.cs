using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetDebitValueWalletTransactionResponse: BaseResponse
    {
        public List<WalletTransactionModel> ListWalletTransaction { get; set; }

        public decimal TotalDebitValue { get; set; }
    }
}
