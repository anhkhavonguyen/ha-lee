using System;

namespace Harvey.EventBus.Events.Products
{
    public class ProductUpdatedEvent : EventBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IndexName { get; set; }
        public string IndexingValue { get; set; }
        public Guid CategoryId { get; set; }

        public ProductUpdatedEvent()
        {

        }
        public ProductUpdatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
