using System;
using Harvey.Domain;

namespace Harvey.PIM.MarketingAutomation
{
    public class FeedItemBase : EntityBase
    {
        public Guid CorrelationId { get; set; }
    }
}
