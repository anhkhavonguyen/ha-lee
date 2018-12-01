using Harvey.Message.Notifications;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendAllSMSNotificationCommandHandler;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendExpiryRewardPointNotificationCommandHandler.Model;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Notifications
{
    public class SendExpiryRewardPointNotificationConsumer : IConsumer<SendSmsExpiryRewardPointNotificationCommand>
    {
        private readonly ISendExpiryRewardPointNotificationCommand _sendExpiryRewardPointNotificationCommand;

        public SendExpiryRewardPointNotificationConsumer(ISendExpiryRewardPointNotificationCommand sendExpiryRewardPointNotificationCommand)
        {
            _sendExpiryRewardPointNotificationCommand = sendExpiryRewardPointNotificationCommand;
        }

        public async Task Consume(ConsumeContext<SendSmsExpiryRewardPointNotificationCommand> context)
        {
            var customersIncludeExpiryRewardPoint = context.Message.CustomersIncludeExpiryRewardPoint;
            var request = new SendExpiryRewardPointNotificationRequest();
            request.SendCustomersIncludeExpiryRewardPointModel = customersIncludeExpiryRewardPoint.Select(x => new SendCustomerIncludeExpiryRewardPointModel
            {
                Phone = x.Phone,
                ExpiringPoints = x.ExpiringPoints,
                AcronymBrandTitle = x.AcronymBrandTitle,
                ExpiredDate = x.ExpiredDate,
                BrandHomeLinkUrl = x.BrandHomeLinkUrl
            }).ToList();

            await _sendExpiryRewardPointNotificationCommand.Execute(request);
        }
    }
}
