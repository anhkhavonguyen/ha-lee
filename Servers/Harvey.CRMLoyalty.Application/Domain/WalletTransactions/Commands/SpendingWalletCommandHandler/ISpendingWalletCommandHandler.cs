using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.SpendingWalletCommandHandler
{
    public interface ISpendingWalletCommandHandler
    {
        Task<decimal> ExecuteAsync(SpendingWalletCommand topUpWalletCommand);
    }
}
