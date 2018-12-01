using System;

namespace Harvey.EventBus
{
    public interface IPublisher
    {
        string Name { get; }
        Guid? CorrelationId { get; set; }
    }
}
