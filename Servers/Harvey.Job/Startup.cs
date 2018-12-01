using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Harvey.Job.Jobs.MembershipTransactions;
using Harvey.Job.Jobs.Notifications;
using Harvey.Job.Jobs.PointTransactions;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Harvey.Job
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }
        private IContainer _container;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddHangfire(cfg=> {
                cfg.UsePostgreSqlStorage(_configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddScoped<SendPendingSMS>();
            services.AddScoped<ExpiryPointCommandSettle>();
            services.AddScoped<ExpiryMembershipNotificationCommandSettle>();
            services.AddScoped<ExpiryRewardPointNotificationCommandSettle>();
            
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c =>
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    var host = sbc.Host(new Uri(_configuration["RabbitMqConfig:RabbitMqUrl"]), h =>
                    {
                        h.Username(_configuration["RabbitMqConfig:Username"]);
                        h.Password(_configuration["RabbitMqConfig:Password"]);
                        h.Heartbeat(10);
                    });
                });
            }).As<IBus>()
            .As<IBusControl>()
            .As<IPublishEndpoint>()
            .SingleInstance();
            containerBuilder.Populate(services);

            _container = containerBuilder.Build();

            var serviceProvider = new AutofacServiceProvider(_container);
            IoC.SetContainer(_container);

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            GlobalConfiguration.Configuration.UseActivator(new ContainerJobActivator(serviceProvider));

            app.UseHangfireServer();
            app.UseHangfireDashboard("/dashboard", new DashboardOptions {
                Authorization = new List<IDashboardAuthorizationFilter> {
                    new DashboardAuthorizationFilter()
                },
            });

            RecurringJob.AddOrUpdate<SendPendingSMS>(sendPendingSMS => sendPendingSMS.Execute(), Cron.MinuteInterval(10));
            RecurringJob.AddOrUpdate<ExpiryPointCommandSettle>(expiryPointCommandSettle => expiryPointCommandSettle.Execute(), Cron.Daily(23,59));
            RecurringJob.AddOrUpdate<ExpiryMembershipNotificationCommandSettle>(settle => settle.Execute(), Cron.Daily(23, 59));
            RecurringJob.AddOrUpdate<ExpiryRewardPointNotificationCommandSettle>(settle => settle.Execute(), Cron.Daily(23,59));
            app.UseMvc();
        }
    }
}
