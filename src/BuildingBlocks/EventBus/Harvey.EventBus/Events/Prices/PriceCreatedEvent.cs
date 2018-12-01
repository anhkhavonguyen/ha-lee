using System;

namespace Harvey.EventBus.Events.Prices
{
    public sealed class PriceCreatedEvent : EventBase
    {
        public Guid VariantId { get; set; }
        public Guid PriceId { get; set; }
        public float ListPrice { get; set; }
        public float StaffPrice { get; set; }
        public float MemberPrice { get; set; }

        public PriceCreatedEvent()
        {

        }
        public PriceCreatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
