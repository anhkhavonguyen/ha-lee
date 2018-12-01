using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Queries.GetWalletTransactionsByOutlet.Model
{
    public class GetWalletTransactionsByOutletResponse : BaseResponse
    {
        public List<WalletTransactionModel> ListWalletTransaction { get; set; }
    }
}
