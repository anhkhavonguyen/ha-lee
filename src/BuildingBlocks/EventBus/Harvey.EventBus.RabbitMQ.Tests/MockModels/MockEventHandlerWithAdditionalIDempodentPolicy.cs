using System.Collections.Generic;
using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Policies;
using Harvey.Polly;
using Microsoft.Extensions.Logging;

namespace Harvey.EventBus.RabbitMQ.Tests.MockModels
{
    public class MockEventHandlerWithAdditionalIDempodentPolicy : MockEventHandler
    {
        protected override List<IIdempotentPolicy<MockEvent>> AdditionalIdempotentPolicies => new List<IIdempotentPolicy<MockEvent>>() { new MockIdempotentPolicy() };

        public MockEventHandlerWithAdditionalIDempodentPolicy(IEventStore eventStore, ILogger<MockEventHandler> logger) : base(eventStore, logger)
        {
        }
    }
}
