using Harvey.Message.Notifications;
using Harvey.Notification.Application.Domains.Accounts.Commands.ReSendSignUpLink;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Accounts
{
    public class ReSendSignUpLinkConsumer : IConsumer<ReSendSignUpLinkCommand>
    {
        private readonly IReSendSignUpLinkCommandHandler _reSendSignUpLinkCommandHandler;
        public ReSendSignUpLinkConsumer(IReSendSignUpLinkCommandHandler reSendSignUpLinkCommandHandler)
        {
            _reSendSignUpLinkCommandHandler = reSendSignUpLinkCommandHandler;
        }
        public async Task Consume(ConsumeContext<ReSendSignUpLinkCommand> context)
        {
            var request = new ReSendSignUpLinkCommandRequest();
            request.CountryCode = context.Message.CountryCode;
            request.PhoneNumber = context.Message.PhoneNumber;
            request.SignUpShortLink = context.Message.SignUpShortLink;
            request.PIN = context.Message.PIN;
            request.OutletName = context.Message.OutletName;

            await _reSendSignUpLinkCommandHandler.ExecuteAsync(request);
        }
    }
}
