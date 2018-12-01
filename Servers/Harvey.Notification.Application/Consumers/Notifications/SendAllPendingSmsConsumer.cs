using Harvey.Message.Notifications;
using Harvey.Notification.Application.Domains.Notifications.Commands.SendAllSMSNotificationCommandHandler;
using MassTransit;
using System.Threading.Tasks;

namespace Harvey.Notification.Application.Consumers.Notifications
{
    public class SendAllPendingSmsConsumer : IConsumer<SendAllPendingSmsCommand>
    {
        private readonly ISendAllSMSNotificationCommandHanlder _sendAllSMSNotificationCommandHanlder;

        public SendAllPendingSmsConsumer(ISendAllSMSNotificationCommandHanlder sendAllSMSNotificationCommandHanlder)
        {
            _sendAllSMSNotificationCommandHanlder = sendAllSMSNotificationCommandHanlder;
        }

        public async Task Consume(ConsumeContext<SendAllPendingSmsCommand> context)
        {
            await Task.Factory.StartNew(() => {
                _sendAllSMSNotificationCommandHanlder.Execute();
            });
        }
    }
}
