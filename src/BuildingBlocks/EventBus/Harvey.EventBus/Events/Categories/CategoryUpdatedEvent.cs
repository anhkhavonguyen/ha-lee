using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.EventBus.Events.Categories
{
    public class CategoryUpdatedEvent : EventBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryUpdatedEvent()
        {

        }
        public CategoryUpdatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
