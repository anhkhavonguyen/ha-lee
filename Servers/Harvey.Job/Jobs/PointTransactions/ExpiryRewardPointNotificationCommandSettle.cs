using Harvey.Message.PointTransactions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;

namespace Harvey.Job.Jobs.PointTransactions
{
    public class ExpiryRewardPointNotificationCommandSettle
    {
        public void Execute()
        {
            IBus _bus = IoC.Resolve<IBus>();
            IConfiguration _configuration = IoC.Resolve<IConfiguration>();

            ISendEndpoint sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "expiry_reward_point_notification_command_settle"))).Result;
            sendEndpointTask.Send<ExpiryRewardPointNotificationMessage>(new
            {
                Date = DateTime.UtcNow
            }).Wait();
        }
    }
}
