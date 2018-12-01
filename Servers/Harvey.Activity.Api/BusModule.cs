using Autofac;
using GreenPipes;
using MassTransit;
using System;

namespace Harvey.Activity.Api
{
    public class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var _configuration = context.Resolve<Microsoft.Extensions.Configuration.IConfiguration>();
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    sbc.AutoDelete = true;
                    sbc.UseRetry(r => r.Interval(3, TimeSpan.FromSeconds(10)));
                    var host = sbc.Host(new Uri(_configuration["RabbitMqConfig:RabbitMqUrl"]), h =>
                    {
                        h.Username(_configuration["RabbitMqConfig:Username"]);
                        h.Password(_configuration["RabbitMqConfig:Password"]);
                        h.Heartbeat(10);
                    });
                    sbc.ReceiveEndpoint(host, "logging_activity_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                });
            }).As<IBusControl>()
            .As<IBus>()
            .SingleInstance();
        }
    }
}
