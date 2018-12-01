using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public interface IGetDebitValueWalletTransactionQuery
    {
        GetDebitValueWalletTransactionResponse Execute(GetDebitValueWalletTransactionRequest request);
    }
}
