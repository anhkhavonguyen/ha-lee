using System;

namespace Harvey.EventBus.Publishers
{
    public class LoggingPublisher : IPublisher
    {
        public string Name => "harvey_logging_publisher_queue";

        public Guid? CorrelationId { get; set; }
    }
}
