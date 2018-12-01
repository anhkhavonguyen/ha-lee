using System;
using Harvey.EventBus;
using Harvey.EventBus.Abstractions;
using Harvey.PIM.MarketingAutomation.Connectors;
using Harvey.PIM.MarketingAutomation.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.PIM.MarketingAutomation
{
    public class SyncServiceBuilder
    {
        private readonly IEventBus _eventBus;
        private readonly ConnectorInfo _connectorInfo;
        private readonly IServiceProvider _serviceProvider;
        private readonly SyncType _syncType;
        public SyncServiceBuilder(SyncType syncType, IServiceProvider serviceProvider, ConnectorInfo connectorInfo)
        {
            _syncType = syncType;
            _serviceProvider = serviceProvider;
            _eventBus = _serviceProvider.GetService<IEventBus>();
            _connectorInfo = connectorInfo;
        }
        public SyncServiceBuilder UseSyncHandler<TEvent, TEventHandler>()
            where TEvent : EventBase
            where TEventHandler : EventHandlerBase<TEvent>
        {
            _eventBus.AddSubcription<TEvent, TEventHandler>($"channel_{_connectorInfo.CorrelationId.ToString().Replace("-", string.Empty)}_{_syncType.ToString()}_operation", _connectorInfo.CorrelationId);
            return this;
        }
    }
}
