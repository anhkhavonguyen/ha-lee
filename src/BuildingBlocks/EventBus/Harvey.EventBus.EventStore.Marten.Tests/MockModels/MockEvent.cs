using Harvey.Domain;

namespace Harvey.EventBus.EventStore.Marten.Tests.MockModels
{
    public class MockEvent : EventBase
    {
        public MockEvent()
        {
        }

        public MockEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
