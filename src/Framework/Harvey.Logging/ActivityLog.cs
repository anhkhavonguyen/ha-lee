using System;

namespace Harvey.Logging
{
    public class ActivityLog : LogEntry
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Context { get; set; }
        public DateTime Date { get; set; }

    }
}
