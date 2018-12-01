using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Queries
{
    public interface IGetDebitValuePointTransactionQuery
    {
        GetDebitValuePointTransactionResponse Execute(GetDebitValuePointTransactionRequest request);
    }
}
