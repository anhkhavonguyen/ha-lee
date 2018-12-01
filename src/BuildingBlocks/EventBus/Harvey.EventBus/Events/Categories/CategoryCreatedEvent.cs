
namespace Harvey.EventBus.Events.Categories
{
    public class CategoryCreatedEvent : EventBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryCreatedEvent()
        {

        }
        public CategoryCreatedEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
