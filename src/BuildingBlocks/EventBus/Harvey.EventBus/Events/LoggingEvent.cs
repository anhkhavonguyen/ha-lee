using Harvey.Logging;
using Microsoft.Extensions.Logging;

namespace Harvey.EventBus.Events
{
    public class LoggingEvent : EventBase
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public Application Application { get; set; }
        public LoggingEvent()
        {
        }

        public LoggingEvent(string aggregateId) : base(aggregateId)
        {
        }
    }
}
