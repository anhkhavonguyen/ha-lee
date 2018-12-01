using Harvey.Message.Notifications;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryMembershipNotificationCommandHandler.Model;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Notifications
{
    public class SendExpiryMembershipNotificationConsumer : IConsumer<SendSmsExpiryMembershipNotificationCommand>
    {
        private readonly ISendExpiryMembershipNotificationCommand _sendExpiryMembershipNotificationCommand;

        public SendExpiryMembershipNotificationConsumer(ISendExpiryMembershipNotificationCommand sendExpiryMembershipNotificationCommand)
        {
            _sendExpiryMembershipNotificationCommand = sendExpiryMembershipNotificationCommand;
        }

        public async Task Consume(ConsumeContext<SendSmsExpiryMembershipNotificationCommand> context)
        {
            var expiryMemberships = context.Message.ExpiryMemberships;
            var request = new SendExpiryMembershipNotificationRequest();
            request.SendExpiryMemberships = expiryMemberships.Select(x => new SendExpiryMembershipModel
            {
                ExpiryMembershipDate = x.ExpiredMembershipDate,
                Phone = x.Phone,
                AcronymBrandTitle = x.AcronymBrandTitle,
                BrandHomeLinkUrl = x.BrandHomeLinkUrl
            }).ToList();

            await _sendExpiryMembershipNotificationCommand.Execute(request);
        }
    }
}
