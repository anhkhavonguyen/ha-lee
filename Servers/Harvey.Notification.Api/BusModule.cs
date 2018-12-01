using Autofac;
using MassTransit;
using System;

namespace Harvey.Notification.Api
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
                    sbc.ReceiveEndpoint(host, "send_email_forgot_password_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "send_sms_forgot_password_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "send_pin_to_phone_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "send_all_pending_sms", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "init_member_account_completed_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "resend_sign_up_link", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "send_sms_change_phone_number_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "send_sms_expiry_membership_notification_queue", e =>
                    {
                        e.LoadFrom(context);
                    });
                    sbc.ReceiveEndpoint(host, "send_sms_expiry_reward_point_notification_queue", e =>
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
