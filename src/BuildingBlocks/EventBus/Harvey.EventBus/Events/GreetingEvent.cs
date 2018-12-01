using Harvey.Domain;

namespace Harvey.EventBus.Events
{
    public class GreetingEvent : EventBase
    {
        public GreetingEvent()
        {
        }

        public GreetingEvent(string aggregateId) : base(aggregateId)
        {
        }

        public string Greeting { get; set; }
    }
}
