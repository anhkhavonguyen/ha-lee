using System;
using System.Linq;
using Harvey.EventBus.Abstractions;
using Harvey.Job;
using Harvey.PIM.MarketingAutomation.Connectors;

namespace Harvey.PIM.MarketingAutomation
{
    public class ApplicationBuilder
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        private readonly IServiceProvider _serviceProvider;
        private readonly IJobManager _jobManager;
        public ApplicationBuilder(IServiceProvider serviceProvider, IJobManager jobManager, ConnectorInfoCollection connectorInfos)
        {
            _serviceProvider = serviceProvider;
            _connectorInfos = connectorInfos;
            _jobManager = jobManager;
        }

        public ApplicationBuilder AddConnector(Guid id, string name, IEventBus eventBus, Action<ConnectorBuilder> registration)
        {
            var connector = _connectorInfos.FirstOrDefault(x => x.Name == name);
            if (connector == null)
            {
                connector = new ConnectorInfo(id, name);
                _connectorInfos.Add(connector);
            }
            var connectorBuilder = new ConnectorBuilder(_serviceProvider, _jobManager, connector);
            registration(connectorBuilder);
            return this;
        }

        public ConnectorInfoCollection GetConnectors()
        {
            return _connectorInfos;
        }
    }
}
