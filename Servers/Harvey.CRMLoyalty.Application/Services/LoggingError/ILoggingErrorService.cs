using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Services.LoggingError;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Services
{
    public interface ILoggingErrorService
    {
        string LogError(ErrorRequest request);
        string LogError(string userId, Exception ex, bool isBackEndSource);
        ExceptionResponse GetErrorLog(ErrorLogRequest request);
    }
}
