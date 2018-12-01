using System;

namespace Harvey.EventBus.Publishers
{
    public class DefaultPublisher : IPublisher
    {
        public string Name { get; }
        public Guid? CorrelationId { get; set; }

        public DefaultPublisher()
        {
            Name = "harvey_default_publisher_queue";
        }

        public DefaultPublisher(string publisher)
        {
            Name = publisher;
        }

    }

}
