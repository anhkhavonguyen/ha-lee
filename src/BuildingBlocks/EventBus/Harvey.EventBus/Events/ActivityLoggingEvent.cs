using Harvey.Logging;

namespace Harvey.EventBus.Events
{
    public class ActivityLoggingEvent : EventBase
    {
        public ActivityLog ActivityLog { get; }
        public ActivityLoggingEvent(ActivityLog activityLog)
        {
            ActivityLog = activityLog;
        }
        public ActivityLoggingEvent()
        {
        }

        public ActivityLoggingEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
