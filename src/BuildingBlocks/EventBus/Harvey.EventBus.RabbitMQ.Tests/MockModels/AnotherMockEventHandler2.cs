using Harvey.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Harvey.EventBus.RabbitMQ.Tests.MockModels
{
    public class AnotherMockEventHandler2 : EventHandlerBase<AnotherMockEvent>
    {
        public bool HasExecuted = false;
        public AnotherMockEventHandler2(IEventStore eventStore, ILogger<AnotherMockEventHandler2> logger) : base(eventStore, logger)
        {

        }
        protected override async Task ExecuteAsync(AnotherMockEvent @event)
        {
            HasExecuted = true;
            await Task.CompletedTask;
        }
    }
}
