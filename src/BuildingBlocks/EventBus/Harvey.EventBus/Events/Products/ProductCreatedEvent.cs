using System;

namespace Harvey.EventBus.Events.Products
{
    public class ProductCreatedEvent : EventBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FieldTemplateId { get; set; }
        public Guid CategoryId { get; set; }
        public string IndexName { get; set; }
        public string IndexingValue { get; set; }

        public ProductCreatedEvent()
        {

        }
        public ProductCreatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
