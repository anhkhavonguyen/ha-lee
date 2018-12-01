using Harvey.Message.Notifications;
using Harvey.Notification.Application.Domains.Accounts.Commands.SendSmsChangePhoneNumber;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Accounts
{
    public class SendSmsChangePhoneNumberConsumer : IConsumer<SendSmsChangePhoneNumberCommand>
    {
        private ISendSmsChangePhoneNumberCommandHandler _sendSmsChangePhoneNumberCommandHandler;
        public SendSmsChangePhoneNumberConsumer(ISendSmsChangePhoneNumberCommandHandler sendSmsChangePhoneNumberCommandHandler)
        {
            _sendSmsChangePhoneNumberCommandHandler = sendSmsChangePhoneNumberCommandHandler;
        }
        public async Task Consume(ConsumeContext<SendSmsChangePhoneNumberCommand> context)
        {
            var message = context.Message;
            var command = new SendSmsChangePhoneNumberCommandRequest();
            command.LoginShortLink = message.LoginShortLink;
            command.ResetPasswordShortLink = message.ResetPasswordShortLink;
            command.UpdateProfileShortLink = message.UpdateProfileShortLink;
            command.NewPhoneNumber = message.NewPhoneNumber;
            command.NewPhoneCountryCode = message.NewPhoneCountryCode;
            command.UpdatedBy = message.UpdatedBy;
            command.AcronymBrandName = message.AcronymBrandName;

            await _sendSmsChangePhoneNumberCommandHandler.ExecuteAsync(command);
        }
    }
}
