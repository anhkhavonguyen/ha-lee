using Harvey.Message.Accounts;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendSMSNotificationForgotPasswordAccount;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Accounts
{
    public class SendForgotPasswordSMSConsumer : IConsumer<SendForgotPasswordSMSMessage>
    {
        private readonly ISendSMSForgotPasswordAccountCommandHandler _sendSMSForgotPasswordAccountCommandHandler;

        public SendForgotPasswordSMSConsumer(ISendSMSForgotPasswordAccountCommandHandler sendForgotPasswordAccountCommandHandler)
        {
            _sendSMSForgotPasswordAccountCommandHandler = sendForgotPasswordAccountCommandHandler;
        }

        public async Task Consume(ConsumeContext<SendForgotPasswordSMSMessage> context)
        {
            await _sendSMSForgotPasswordAccountCommandHandler.ExecuteAsync(new SendSMSForgotPasswordAccountCommand
            {
                PhoneCountryCode = context.Message.PhoneCountryCode,
                PhoneNumber = context.Message.PhoneNumber,
                Link = context.Message.Link,
                Title = context.Message.Title,
                AcronymBrandName = context.Message.AcronymBrandName,
                OutletName = context.Message.OutletName
            });
        }
    }
}
