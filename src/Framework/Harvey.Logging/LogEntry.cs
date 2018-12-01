using Harvey.Domain;
using Microsoft.Extensions.Logging;

namespace Harvey.Logging
{
    public class LogEntry : EntityBase
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public Application Application { get; set; }
    }
}
