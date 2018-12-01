using Harvey.Message.Accounts;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendPINToNumberPhone;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Accounts
{
    public class SendPINToNumberPhoneConsumer : IConsumer<SendPINToNumberPhoneMessage>
    {
        private readonly ISendPINToNumberPhoneCommandHandler _sendPINToNumberPhoneCommandHandler;

        public SendPINToNumberPhoneConsumer(ISendPINToNumberPhoneCommandHandler sendPINToNumberPhoneCommandHandler)
        {
            _sendPINToNumberPhoneCommandHandler = sendPINToNumberPhoneCommandHandler;
        }

        public async Task Consume(ConsumeContext<SendPINToNumberPhoneMessage> context)
        {
            await _sendPINToNumberPhoneCommandHandler.ExecuteAsync(new SendPINToNumberPhoneCommand
            {
                Pin = context.Message.Pin,
                PhoneCountryCode = context.Message.PhoneCountryCode,
                PhoneNumber = context.Message.PhoneNumber,
                OutletName = context.Message.OutletName,
                AcronymBrandName = context.Message.AcronymBrandName
            });
        }
    }
}
