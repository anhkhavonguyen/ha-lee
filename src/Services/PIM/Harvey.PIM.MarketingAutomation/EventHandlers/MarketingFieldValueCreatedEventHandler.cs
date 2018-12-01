using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.FileldValues;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PIM.MarketingAutomation.EventHandlers
{
    public class MarketingFieldValueCreatedEventHandler : EventHandlerBase<FieldValueCreatedEvent>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEventBus _eventBus;
        public MarketingFieldValueCreatedEventHandler(
            ConnectorInfoCollection connectorInfos,
            IEventBus eventBus,
            IEventStore eventStore,
            ILogger<EventHandlerBase<FieldValueCreatedEvent>> logger) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _eventBus = eventBus;
        }
        protected override async Task ExecuteAsync(FieldValueCreatedEvent @event)
        {
            foreach (var item in _connectorInfos)
            {
                @event.CorrelationId = item.CorrelationId;
                await _eventBus.PublishAsync(new MarketingAutomationEvent<FieldValueCreatedEvent>(@event)
                {
                    CorrelationId = item.CorrelationId
                });
            }
        }
    }
}
