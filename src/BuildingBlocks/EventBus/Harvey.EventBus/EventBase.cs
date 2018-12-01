using Harvey.Domain;
using System;

namespace Harvey.EventBus
{
    public class EventBase : IDomainEvent
    {
        public Guid? CorrelationId { get; set; }
        public Guid Id { get; set; }
        public string AggregateId { get; set; }
        public DateTime LastTimeStamp { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Version { get; set; }

        public EventBase()
        {
            Id = Guid.NewGuid();
        }

        public EventBase(string aggregateId) : this()
        {
            AggregateId = aggregateId;
        }
    }
}
