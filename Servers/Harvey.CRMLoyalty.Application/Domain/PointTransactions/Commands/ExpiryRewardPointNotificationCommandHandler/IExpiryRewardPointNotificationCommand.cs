using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler.Model;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler
{
    public interface IExpiryRewardPointNotificationCommand
    {
        Task ExecuteAsync(ExpiryRewardPointNotificationModel command);
    }
}
