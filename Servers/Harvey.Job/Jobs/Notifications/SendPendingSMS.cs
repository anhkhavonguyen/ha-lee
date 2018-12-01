using Harvey.Message.Notifications;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Harvey.Job.Jobs.Notifications
{
    public class SendPendingSMS
    {
        public void Execute()
        {
            IBus _bus = IoC.Resolve<IBus>();
            IConfiguration _configuration = IoC.Resolve<IConfiguration>();

            ISendEndpoint sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "send_all_pending_sms"))).Result;
            sendEndpointTask.Send<SendAllPendingSmsCommand>(new {
                IsSendAllMessage = true
            }).Wait();
        }
    }
}
