using Microsoft.Extensions.Logging;

namespace Harvey.Logging
{
    public class DatabaseLoggingConfiguration : IDatabaseLoggingConfiguration
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
