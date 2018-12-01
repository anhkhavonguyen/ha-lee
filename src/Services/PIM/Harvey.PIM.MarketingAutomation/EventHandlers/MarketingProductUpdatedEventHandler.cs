using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Products;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PIM.MarketingAutomation.EventHandlers
{
    public sealed class MarketingProductUpdatedEventHandler : EventHandlerBase<ProductUpdatedEvent>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEventBus _eventBus;
        public MarketingProductUpdatedEventHandler(
            ConnectorInfoCollection connectorInfos,
            IEventBus eventBus,
            IEventStore eventStore,
            ILogger<EventHandlerBase<ProductUpdatedEvent>> logger) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(ProductUpdatedEvent @event)
        {
            foreach (var item in _connectorInfos)
            {
                @event.CorrelationId = item.CorrelationId;
                await _eventBus.PublishAsync(new MarketingAutomationEvent<ProductUpdatedEvent>(@event)
                {
                    CorrelationId = item.CorrelationId
                });
            }

        }
    }
}
