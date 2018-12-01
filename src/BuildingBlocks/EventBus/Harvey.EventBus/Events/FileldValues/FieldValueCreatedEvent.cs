using Harvey.Domain;
using System;

namespace Harvey.EventBus.Events.FileldValues
{
    public class FieldValueCreatedEvent : EventBase
    {
        public Guid FieldValueId { get; set; }
        public Guid EntityId { get; set; }
        public Guid FieldId { get; set; }
        public string FieldValue { get; set; }
        public FieldType FieldType { get; set; }
        public string FieldName { get; set; }
        public string Section { get; set; }
        public int OrderSection { get; set; }
        public bool IsVariantField { get; set; }

        public FieldValueCreatedEvent()
        {

        }
        public FieldValueCreatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
