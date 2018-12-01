using Harvey.EventBus.Abstractions;
using Harvey.EventBus.RabbitMQ;
using Harvey.EventBus.RabbitMQ.Policies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Harvey.Logging.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<MasstransitPersistanceConnection>>();
                var endPoint = configuration["EventBusConnection"];
                return new MasstransitPersistanceConnection(new BusCreationRetrivalPolicy(), logger, endPoint);
            });
            services.AddSingleton<IEventBus, MasstransitEventBus>();
            return services;
        }
    }
}
