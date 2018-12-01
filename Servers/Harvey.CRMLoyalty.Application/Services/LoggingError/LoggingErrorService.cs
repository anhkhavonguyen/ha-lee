using System;
using System.Collections.Generic;
using System.Linq;
using Harvey.CRMLoyalty.Api;
using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Extensions.ExceptionExtensions;
using Harvey.CRMLoyalty.Application.Extensions.PagingExtensions;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Services.LoggingError;

namespace Harvey.CRMLoyalty.Application.Services
{
    public class LoggingErrorService : ILoggingErrorService
    {
        private readonly HarveyCRMLoyaltyDbContext _dbContext;

        public LoggingErrorService(HarveyCRMLoyaltyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string LogError(ErrorRequest request)
        {
            if (request == null)
                return null;

            var source = (int)(SourceErrorLog)Enum.Parse(typeof(SourceErrorLog), request.Source, true);
            return WriteLog(request.UserId, request.ErrorCaption, request.ErrorMessage, source);
        }

        public string LogError(string userId, Exception ex, bool isBackEndSource)
        {
            var messageDetail = ex.ToLogString(Environment.StackTrace);
            var source = isBackEndSource ? (int)SourceErrorLog.BackEnd : (int)SourceErrorLog.FrontEnd;
            return WriteLog(userId, ex.Message, messageDetail, source);
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
            }).OrderByDescending(x=>x.CreatedDate).AsQueryable();

            var result = PagingExtensions.GetPaged<ErrorLogEntryModel>(errorLogQuery, request.PageNumber, request.PageSize);
            var response = new ExceptionResponse();
            response.TotalItem = result.TotalItem;
            response.PageSize = result.PageSize;
            response.PageNumber = result.PageNumber;
            response.ListError = result.Results;
            return response;
        }
    }
}
