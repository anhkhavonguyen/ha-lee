using Harvey.EventBus.Abstractions;
using System;
using System.Collections.Generic;

namespace Harvey.PIM.MarketingAutomation.Connectors
{
    public class ConnectorInfo
    {
        public Guid CorrelationId { get; }
        public string Name { get; }
        public List<FeedBase> Feeds { get; set; }

        public ConnectorInfo(Guid correlationId, string name)
        {
            Feeds = new List<FeedBase>();
            CorrelationId = correlationId;
            Name = name;
        }
    }
}
