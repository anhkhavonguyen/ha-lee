using System;
using Harvey.Domain;

namespace Harvey.EventBus.Events.FileldValues
{
    public class FieldValueUpdatedEvent : EventBase
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

        public FieldValueUpdatedEvent()
        {

        }
        public FieldValueUpdatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
