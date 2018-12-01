using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public class GetTotalBalanceWalletTransactionResponse : BaseResponse
    {
        public decimal TotalBalance { get; set; }
    }
}
