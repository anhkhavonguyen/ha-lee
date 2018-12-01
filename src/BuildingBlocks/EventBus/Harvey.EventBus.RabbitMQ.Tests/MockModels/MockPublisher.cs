using System;

namespace Harvey.EventBus.RabbitMQ.Tests.MockModels
{
    public class MockPublisher : IPublisher
    {
        public string Name => "mock_publisher_queue";

        public Guid? CorrelationId { get; set; }
    }
}
