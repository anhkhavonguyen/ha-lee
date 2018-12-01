namespace Harvey.Notification.Application.Entities
{
    public class Template : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string DisplayName { get; set; }
        public string TemplateKey { get; set; }
        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }
    }
}
