using Harvey.Activity.Api;
using Harvey.Activity.Application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Services.LoggingError
{
    public class LoggingErrorService : ILoggingErrorService
    {
        private readonly HarveyActivityDbContext _dbContext;

        public LoggingErrorService(HarveyActivityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string LogError(ErrorRequest request)
        {
            if (request == null)
                return "-1";

            var source = (int)(SourceErrorLog)Enum.Parse(typeof(SourceErrorLog), request.Source, true);

            return WriteLog(request.UserId, request.ErrorCaption, request.ErrorMessage, source);
        }

        public string LogError(string userId, Exception ex, bool isBackEndSource)
        {
            var source = isBackEndSource ? (int)SourceErrorLog.BackEnd : (int)SourceErrorLog.FrontEnd;
            return WriteLog(userId, ex.Message, ex.InnerException?.Message, source);
        }

        private string WriteLog(string userId, string caption, string detail, int source)
        {
            var entry = new ErrorLogEntry();

            entry.CreatedBy = userId;
            entry.CreatedDate = DateTime.Now;
            entry.Caption = caption;
            entry.Detail = detail;
            entry.ErrorSourceId = source;
            _dbContext.ErrorLogEntries.Add(entry);
            _dbContext.SaveChanges();

            return entry.Id;
        }
    }
}
