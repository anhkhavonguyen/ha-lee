using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.VoidWalletCommandHandler
{
    public class VoidWalletCommand : BaseRequest
    {
        public string WalletTransactionId { get; set; }

        public string IpAddress { get; set; }

        public string VoidByName { get; set; }
    }
}
