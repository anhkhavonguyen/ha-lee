using Harvey.Notification.Api;
using Harvey.Notification.Application.Entities;
using Harvey.Notification.Application.Extensions.PagingExtensions;
using Harvey.Notification.Application.Models;
using System;
using System.Linq;

namespace Harvey.Notification.Application.Services.LoggingError
{
    public class LoggingErrorService : ILoggingErrorService
    {
        private readonly HarveyNotificationDbContext _dbContext;

        public LoggingErrorService(HarveyNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public long LogError(ErrorRequest request)
        {
            if (request == null)
                return -1;

            var source = (int)(SourceErrorLog)Enum.Parse(typeof(SourceErrorLog), request.Source, true);

            return WriteLog(request.UserId, request.ErrorCaption, request.ErrorMessage, source);
        }

        public long LogError(string userId, Exception ex, bool isBackEndSource)
        {
            var source = isBackEndSource ? (int)SourceErrorLog.BackEnd : (int)SourceErrorLog.FrontEnd;
            return WriteLog(userId, ex.Message, ex.InnerException?.Message, source);
        }

        private long WriteLog(string userId, string caption, string detail, int source)
        {
            var entry = new ErrorLogEntry();

            entry.CreatedBy = userId;
            entry.CreatedDate = DateTime.Now;
            entry.Caption = caption;
            entry.Detail = detail;
            entry.ErrorLogSourceId = source;
            _dbContext.ErrorLogEntries.Add(entry);
            _dbContext.SaveChanges();

            return entry.Id;
        }

        public ExceptionResponse GetErrorLog(ErrorLogRequest request)
        {
            var errorLogQuery = _dbContext.ErrorLogEntries.Select(x => new ErrorLogEntryModel()
            {
                Id = x.Id,
                Detail = x.Detail,
                Caption = x.Caption,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ErrorLogSource = x.ErrorLogSource.SourceName
            }).AsQueryable();

            var result = PagingExtensions.GetPaged<ErrorLogEntryModel>(errorLogQuery, request.PageNumber, request.PageSize);
            var response = new ExceptionResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListError = result.Results.ToList();
            return response;
        }
    }
}
