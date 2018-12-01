using Autofac;
using MassTransit;
using System;

namespace Harvey.CRMLoyalty.Api
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
                    sbc.ReceiveEndpoint(host, "update_member_profile_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "update_full_customer_infomation_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "update_customer_profile_after_init_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "update_gender_value_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "change_phone_number_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "expiry_point_command_settle", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "expiry_membership_notification_command_settle", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "expiry_reward_point_notification_command_settle", e =>
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
