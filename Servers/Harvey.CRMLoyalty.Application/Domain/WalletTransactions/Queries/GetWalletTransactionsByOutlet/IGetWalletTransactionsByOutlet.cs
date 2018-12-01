using Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet
{
    public interface IGetWalletTransactionsByOutlet
    {
        GetWalletTransactionsByOutletResponse Execute(GetWalletTransactionsByOutletRequest request);
    }
}
