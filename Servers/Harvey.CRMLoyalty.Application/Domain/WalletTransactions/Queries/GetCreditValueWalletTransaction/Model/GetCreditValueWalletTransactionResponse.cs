using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetCreditValueWalletTransactionResponse : BaseResponse
    {
        public List<WalletTransactionModel> ListWalletTransaction { get; set; }

        public decimal TotalCreditValue { get; set; }
    }
}
