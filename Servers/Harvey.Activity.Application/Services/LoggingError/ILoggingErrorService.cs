using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Services.LoggingError
{
    public interface ILoggingErrorService
    {
        string LogError(ErrorRequest request);
        string LogError(string userId, Exception ex, bool isBackEndSource);
    }
}
