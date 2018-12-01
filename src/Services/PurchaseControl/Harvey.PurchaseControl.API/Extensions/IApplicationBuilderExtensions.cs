using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events;
using Harvey.EventBus.Publishers;
using Harvey.PurchaseControl.Application.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.PurchaseControl.API.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.AddSubcription<GreetingEvent, GreetingEventEventHandler>();
            eventBus.AddSubcription<LoggingPublisher, LoggingEvent, LoggingEventHandler>();
            eventBus.Commit();
        }
    }
}
