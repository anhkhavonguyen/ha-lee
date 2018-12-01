using Harvey.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.EventBus.RabbitMQ.Tests.MockModels
{
    public class MockEventHandler : EventHandlerBase<MockEvent>
    {
        public bool HasExecuted = false;
        public MockEventHandler(IEventStore eventStore, ILogger<MockEventHandler> logger) : base(eventStore, logger)
        {

        }
        protected override async Task ExecuteAsync(MockEvent @event)
        {
            HasExecuted = true;
            await Task.CompletedTask;
        }
    }
}
