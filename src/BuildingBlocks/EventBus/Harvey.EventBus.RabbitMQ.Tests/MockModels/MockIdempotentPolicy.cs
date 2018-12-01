using Harvey.Polly;

namespace Harvey.EventBus.RabbitMQ.Tests.MockModels
{
    public class MockIdempotentPolicy : IIdempotentPolicy<MockEvent>
    {
        public void Handle(MockEvent entity)
        {
        }

        public bool ShouldHandle(MockEvent entity)
        {
            return true;
        }
    }
}
