namespace Harvey.EventBus.EventStore.Marten.Tests.MockModels
{
    public class AnotherMockEvent : MockEvent
    {
        public AnotherMockEvent()
        {
        }

        public AnotherMockEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
