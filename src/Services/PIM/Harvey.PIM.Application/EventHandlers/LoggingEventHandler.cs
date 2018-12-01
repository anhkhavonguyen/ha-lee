using System.Threading.Tasks;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Harvey.PIM.Application.EventHandlers
{
    public sealed class LoggingEventHandler : EventHandlerBase<LoggingEvent>
    {
        public LoggingEventHandler(IEventStore eventStore, ILogger<EventHandlerBase<LoggingEvent>> logger) : base(eventStore, logger)
        {
        }

        protected override Task ExecuteAsync(LoggingEvent @event)
        {
            if (@event.Application == Logging.Application.PIM)
            {
                Logger.Log(@event.LogLevel, $" [{@event.Application.ToString()}] {@event.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
