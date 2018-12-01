using Harvey.EventBus.Abstractions;
using Harvey.EventBus.EventStore.Marten;
using Harvey.EventBus.RabbitMQ;
using Harvey.EventBus.RabbitMQ.Policies;
using Harvey.Logging;
using Harvey.Logging.SeriLog;
using Harvey.PurchaseControl.Application.EventHandlers;
using Harvey.PurchaseControl.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace Harvey.PurchaseControl.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEventStore>(sp =>
            {
                return new MartenEventStore(configuration["ConnectionString"]);
            });

            services.AddTransient<GreetingEventEventHandler>();
            services.AddTransient<LoggingEventHandler>();

            services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<MasstransitPersistanceConnection>>();
                var endPoint = configuration["EventBusConnection"];
                return new MasstransitPersistanceConnection(new BusCreationRetrivalPolicy(), logger, endPoint);
            });
            services.AddSingleton<IEventBus, MasstransitEventBus>();
            return services;
        }

        public static IServiceCollection AddServiceDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PurchaseControlDbContext>(options =>
            {
                var connectionString = configuration["ConnectionString"];
                options.UseNpgsql(connectionString);
            });
            return services;
        }

        public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
        {
            new SeriLogger().Initilize(new List<IDatabaseLoggingConfiguration>()
            {
                new DatabaseLoggingConfiguration()
                {
                    ConnectionString = configuration["Logging:DataLogger:ConnectionString"],
                    TableName = configuration["Logging:DataLogger:TableName"],
                    LogLevel = (LogLevel)Enum.Parse( typeof(LogLevel), configuration["Logging:DataLogger:LogLevel"],true)
                }
            }, new List<ICentralizeLoggingConfiguration>() {
                new CentralizeLoggingConfiguration()
                {
                    Url = configuration["Logging:CentralizeLogger:Url"],
                    LogLevel = (LogLevel)Enum.Parse( typeof(LogLevel), configuration["Logging:CentralizeLogger:LogLevel"],true)
                }
            });
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            return services;
        }
    }
}
