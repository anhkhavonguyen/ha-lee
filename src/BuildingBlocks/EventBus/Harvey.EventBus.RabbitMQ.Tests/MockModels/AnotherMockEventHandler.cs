using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Policies;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.EventBus.RabbitMQ.Tests.MockModels
{
    public class AnotherMockEventHandler : EventHandlerBase<MockEvent>
    {
        public bool HasExecuted = false;
        public AnotherMockEventHandler(IEventStore eventStore, ILogger<AnotherMockEventHandler> logger) : base(eventStore, logger)
        {

        }
        protected override async Task ExecuteAsync(MockEvent @event)
        {
            HasExecuted = true;
            await Task.CompletedTask;
        }
    }
}
