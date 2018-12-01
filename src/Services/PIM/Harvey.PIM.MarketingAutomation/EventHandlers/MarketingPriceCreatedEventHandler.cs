using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Prices;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PIM.MarketingAutomation.EventHandlers
{
    public class MarketingPriceCreatedEventHandler : EventHandlerBase<PriceCreatedEvent>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEventBus _eventBus;
        public MarketingPriceCreatedEventHandler(
            ConnectorInfoCollection connectorInfos,
            IEventBus eventBus,
            IEventStore eventStore,
            ILogger<EventHandlerBase<PriceCreatedEvent>> logger) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _eventBus = eventBus;
        }
        protected override async Task ExecuteAsync(PriceCreatedEvent @event)
        {
            foreach (var item in _connectorInfos)
            {
                @event.CorrelationId = item.CorrelationId;
                await _eventBus.PublishAsync(new MarketingAutomationEvent<PriceCreatedEvent>(@event)
                {
                    CorrelationId = item.CorrelationId
                });
            }
        }
    }
}
