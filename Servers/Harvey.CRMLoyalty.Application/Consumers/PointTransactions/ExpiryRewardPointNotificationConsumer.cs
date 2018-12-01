using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.PointTransactions.Commands.ExpiryRewardPointNotificationCommandHandler.Model;
using Harvey.Message.PointTransactions;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Consumers.PointTransactions
{
    public class ExpiryRewardPointNotificationConsumer : IConsumer<ExpiryRewardPointNotificationMessage>
    {
        private readonly IExpiryRewardPointNotificationCommand _expiryRewardPointNotificationCommand;

        public ExpiryRewardPointNotificationConsumer(IExpiryRewardPointNotificationCommand expiryRewardPointNotificationCommand)
        {
            _expiryRewardPointNotificationCommand = expiryRewardPointNotificationCommand;
        }

        public async Task Consume(ConsumeContext<ExpiryRewardPointNotificationMessage> context)
        {
            var command = new ExpiryRewardPointNotificationModel();
            command.Date = context.Message.Date;
            await _expiryRewardPointNotificationCommand.ExecuteAsync(command);
        }
    }
}
