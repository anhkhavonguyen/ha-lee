using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PurchaseControl.Application.EventHandlers
{
    public class GreetingEventEventHandler : EventHandlerBase<GreetingEvent>
    {
        public GreetingEventEventHandler(IEventStore eventStore, ILogger<EventHandlerBase<GreetingEvent>> logger) : base(eventStore, logger)
        {
        }

        protected override Task ExecuteAsync(GreetingEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
