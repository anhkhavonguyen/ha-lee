using Microsoft.Extensions.Logging;

namespace Harvey.Logging
{
    public interface ILoggingConfiguration
    {
        LogLevel LogLevel { get; set; }
    }
}
