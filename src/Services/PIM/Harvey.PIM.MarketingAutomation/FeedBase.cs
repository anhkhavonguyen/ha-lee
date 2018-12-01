using System;

namespace Harvey.PIM.MarketingAutomation
{
    public abstract class FeedBase
    {
        public Guid CorrelationId { get; }
        public string Name { get; }
        public TimeSpan DueTime { get; set; }
        public TimeSpan Interval { get; set; }
        protected FeedBase(Guid correlationId, string name)
        {
            CorrelationId = correlationId;
            Name = name;
        }
        public abstract void Execute();
    }
}
