using Microsoft.Extensions.Logging;

namespace Harvey.Logging
{
    public class CentralizeLoggingConfiguration : ICentralizeLoggingConfiguration
    {
        public string Url { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
