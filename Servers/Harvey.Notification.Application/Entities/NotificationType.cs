using System.Collections.Generic;

namespace Harvey.Notification.Application.Entities
{
    public class NotificationType : BaseEntity<int>
    {
        public string TypeName { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
    }
}
