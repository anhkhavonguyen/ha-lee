using System.Collections.Generic;

namespace Harvey.Activity.Application.Entities
{
    public class ActionType: EntityBase<string>
    {
        public string Name { get; set; }
        public string ActionActivityId { get; set; }
        public virtual ICollection<ActionActivity> ActionActivities { get; set; }
    }
}
