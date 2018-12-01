using Harvey.Message.PointTransactions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Job.Jobs.PointTransactions
{
    public class ExpiryPointCommandSettle
    {
        public void Execute()
        {
            IBus _bus = IoC.Resolve<IBus>();
            IConfiguration _configuration = IoC.Resolve<IConfiguration>();

            ISendEndpoint sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(_configuration["RabbitMqConfig:RabbitMqUrl"], "/", "expiry_point_command_settle"))).Result;
            sendEndpointTask.Send<ExpiryPointCommandMessage>(new
            {
                Date = DateTime.UtcNow
            }).Wait();
        }
    }
}
