using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PurchaseControl.Application.EventHandlers
{
    public sealed class LoggingEventHandler : EventHandlerBase<LoggingEvent>
    {
        public LoggingEventHandler(IEventStore eventStore, ILogger<EventHandlerBase<LoggingEvent>> logger) : base(eventStore, logger)
        {
        }

        protected override async Task ExecuteAsync(LoggingEvent @event)
        {
            Logger.Log(@event.LogLevel, $" [{@event.Application.ToString()}] {@event.Message}");
            await Task.CompletedTask;
        }
    }
}
