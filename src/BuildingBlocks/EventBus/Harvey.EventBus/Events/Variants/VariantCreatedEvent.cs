using System;

namespace Harvey.EventBus.Events.Variants
{
    public class VariantCreatedEvent : EventBase
    {
        public Guid VariantId { get; set; }
        public Guid ProductId { get; set; }
        public VariantCreatedEvent()
        {

        }
        public VariantCreatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
