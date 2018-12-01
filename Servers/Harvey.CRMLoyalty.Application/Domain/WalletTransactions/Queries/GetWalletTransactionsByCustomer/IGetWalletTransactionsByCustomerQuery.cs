using Harvey.CRMLoyalty.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries
{
    public interface IGetWalletTransactionsByCustomerQuery
    {
        GetWalletTransactionsByCustomerResponse Execute(GetWalletTransactionsByCustomerRequest request);
        GetWalletTransactionsByCustomerResponse GetByMember(GetWalletTransactionsByCustomerRequest request);
    }
}
