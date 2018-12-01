using Harvey.Message.Accounts;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendEmailNotificationForgotPasswordAccount;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Accounts
{
    public class SendForgotPasswordEmailConsumer : IConsumer<SendForgotPasswordEmailMessage>
    {
        private readonly ISendEmailForgotPasswordAccountCommandHandler _sendForgotPasswordAccountCommandHandler;

        public SendForgotPasswordEmailConsumer(ISendEmailForgotPasswordAccountCommandHandler sendForgotPasswordAccountCommandHandler)
        {
            _sendForgotPasswordAccountCommandHandler = sendForgotPasswordAccountCommandHandler;
        }

        public async Task Consume(ConsumeContext<SendForgotPasswordEmailMessage> context)
        {
            await _sendForgotPasswordAccountCommandHandler.ExecuteAsync(new SendEmailForgotPasswordAccountCommand
            {
                Email = context.Message.Email,
                ShortLink = context.Message.ShortLink,
                Title = context.Message.Title,
                LastName = context.Message.LastName,
                FirstName = context.Message.FirstName,
                AcronymBrandName = context.Message.AcronymBrandName,
                BrandName = context.Message.BrandName
            });
        }
    }
}
