using System;

namespace Harvey.EventBus
{
    public class MarketingAutomationEvent<TEvent> : EventBase
       where TEvent : EventBase
    {
        public TEvent InnerEvent { get; set; }
        public MarketingAutomationEvent(TEvent @event) : base()
        {
            InnerEvent = @event;
        }
    }
}
