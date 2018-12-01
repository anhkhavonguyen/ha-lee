using Microsoft.AspNet.SignalR.Client;
using System.Collections;
using System.Text;

namespace Harvey.Exception.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetTraceLog(this System.Exception ex, string message = null)
        {
            var log = new StringBuilder();
            if (!string.IsNullOrEmpty(message))
            {
                log.AppendLine(message);
            }
            log.AppendLine();
            var exception = ex;
            while (exception != null)
            {
                log.AppendLine("Messages: " + exception.Message);
                log.AppendLine("Exception: " + exception.GetType().AssemblyQualifiedName);
                var httpCLientException = exception as HttpClientException;
                if (httpCLientException != null)
                {
                    log.AppendLine("Error messages:");
                    log.AppendLine(httpCLientException.Message);
                }
                log.AppendLine(exception.StackTrace);
                log.AppendLine();
                log.Append("Data: ");
                foreach (DictionaryEntry item in exception.Data)
                {
                    log.AppendLine("-------------------------------------------------------");
                    log.AppendFormat("{0}='{1}';", item.Key, item.Value);
                }
                log.AppendLine();
                exception = exception.InnerException;
            }
            return log.ToString();
        }
    }
}
