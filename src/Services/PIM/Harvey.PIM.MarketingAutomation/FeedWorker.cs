using System;
using System.Linq;
using Harvey.Job;
using Harvey.PIM.MarketingAutomation.Connectors;

namespace Harvey.PIM.MarketingAutomation
{
    public class FeedWorker : IWorker
    {
        private readonly ConnectorInfoCollection _connectorInfos;
        public FeedWorker(ConnectorInfoCollection connectorInfos)
        {
            _connectorInfos = connectorInfos;
        }
        public void Execute(Guid correlationId, string jobName)
        {
            var connector = _connectorInfos.FirstOrDefault(x => x.CorrelationId == correlationId);
            if (connector == null)
            {
                return;
            }

            var feed = connector.Feeds.FirstOrDefault(x => x.Name == jobName);
            if (feed == null)
            {
                return;
            }
            feed.Execute();
        }
    }
}
