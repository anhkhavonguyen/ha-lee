using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Notification.Application.Services.LoggingError
{
    public interface ILoggingErrorService
    {
        long LogError(ErrorRequest request);
        long LogError(string userId, Exception ex, bool isBackEndSource);
        ExceptionResponse GetErrorLog(ErrorLogRequest request);
    }
}
