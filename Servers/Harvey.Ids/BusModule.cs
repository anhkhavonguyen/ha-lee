using Autofac;
using MassTransit;
using System;

namespace Harvey.Ids
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
                    var host = sbc.Host(new Uri(_configuration["RabbitMqConfig:RabbitMqUrl"]), h =>
                    {
                        h.Username(_configuration["RabbitMqConfig:Username"]);
                        h.Password(_configuration["RabbitMqConfig:Password"]);
                        h.Heartbeat(10);
                    });
                    sbc.ReceiveEndpoint(host, "init_member_account_profile", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "active_customer_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "reactive_customer_queue", e =>
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
