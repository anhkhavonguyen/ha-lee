using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.WalletTransactions.Commands.TopUpWalletCommandHandler
{
    public interface ITopUpWalletCommandHandler
    {
        Task<decimal> ExecuteAsync(TopUpWalletCommand topUpWalletCommand);
    }
}
