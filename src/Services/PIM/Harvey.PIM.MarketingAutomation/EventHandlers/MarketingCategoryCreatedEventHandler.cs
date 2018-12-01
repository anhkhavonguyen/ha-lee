using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events.Categories;
using Harvey.PIM.MarketingAutomation.Connectors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.PIM.MarketingAutomation.EventHandlers
{
    public class MarketingCategoryCreatedEventHandler : EventHandlerBase<CategoryCreatedEvent>
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IEventBus _eventBus;
        public MarketingCategoryCreatedEventHandler(
            ConnectorInfoCollection connectorInfos,
            IEventBus eventBus,
            IEventStore eventStore,
            ILogger<EventHandlerBase<CategoryCreatedEvent>> logger) : base(eventStore, logger)
        {
            _connectorInfos = connectorInfos;
            _eventBus = eventBus;
        }
        protected override async Task ExecuteAsync(CategoryCreatedEvent @event)
        {
            foreach (var item in _connectorInfos)
            {
                @event.CorrelationId = item.CorrelationId;
                await _eventBus.PublishAsync(new MarketingAutomationEvent<CategoryCreatedEvent>(@event)
                {
                    CorrelationId = item.CorrelationId
                });
            }
        }
    }
}
