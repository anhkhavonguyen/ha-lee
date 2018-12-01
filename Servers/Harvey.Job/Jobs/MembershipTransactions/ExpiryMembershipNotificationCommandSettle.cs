using Harvey.Message.MembershipTransactions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;

namespace Harvey.Job.Jobs.MembershipTransactions
{
    public class ExpiryMembershipNotificationCommandSettle
    {
        public void Execute()
        {
            IBus _bus = IoC.Resolve<IBus>();
            IConfiguration _configuration = IoC.Resolve<IConfiguration>();

            ISendEndpoint sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "expiry_membership_notification_command_settle"))).Result;
            sendEndpointTask.Send<ExpiryMembershipNotificationMessage>(new
            {
                Date = DateTime.UtcNow
            }).Wait();
        }
    }
}
