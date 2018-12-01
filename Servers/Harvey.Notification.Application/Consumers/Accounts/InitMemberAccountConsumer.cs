using Harvey.Message.Accounts;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendNotificationInitAccount;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Accounts
{
    public class InitMemberAccountConsumer : IConsumer<InitMemberAccountCompletedMessage>
    {
        private readonly ISendNotificationInitAccountCommandHandler _sendNotificationInitAccount;
        public InitMemberAccountConsumer(ISendNotificationInitAccountCommandHandler sendNotificationInitAccount)
        {
            _sendNotificationInitAccount = sendNotificationInitAccount;
        }
        public async Task Consume(ConsumeContext<InitMemberAccountCompletedMessage> context)
        {
            await _sendNotificationInitAccount.ExecuteAsync(context.Message.PhoneCountryCode, context.Message.PhoneNumber, context.Message.OutletName, context.Message.SignUpShortLink,
                context.Message.Pin);
        }
    }
}
