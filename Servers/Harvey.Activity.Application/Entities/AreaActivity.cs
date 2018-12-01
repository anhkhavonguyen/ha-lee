using System.Collections.Generic;

namespace Harvey.Activity.Application.Entities
{
    public class AreaActivity : EntityBase<string>
    {
        public string AreaPath { get; set; }
        public string Description { get; set; }
        public string ActionActivityId { get; set; }
        public virtual ICollection<ActionActivity> ActionActivities { get; set; }
    }
}
