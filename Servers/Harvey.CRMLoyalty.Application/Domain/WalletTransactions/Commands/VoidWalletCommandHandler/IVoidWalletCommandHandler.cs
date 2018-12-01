using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.VoidWalletCommandHandler
{
    public interface IVoidWalletCommandHandler
    {
        Task<decimal> ExecuteAsync(VoidWalletCommand command);
    }
}
