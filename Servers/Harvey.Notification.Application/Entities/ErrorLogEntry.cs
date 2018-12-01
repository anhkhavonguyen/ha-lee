namespace Harvey.Notification.Application.Entities
{
    public class ErrorLogEntry: BaseEntity<long>
    {
        public string Detail { get; set; }
        public string Caption { get; set; }
        public string Source { get; set; }
        public int ErrorLogSourceId { get; set; }
        public virtual ErrorLogSource ErrorLogSource { get; set; }
    }

    public enum SourceErrorLog
    {
        AdminApp = 1,
        MemberApp = 2,
        StoreApp = 3,
        BackEnd = 4,
        FrontEnd = 5
    }
}
