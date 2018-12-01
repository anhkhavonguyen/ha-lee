using Harvey.Notification.Application.Models;
using System.Collections.Generic;

namespace Harvey.Notification.Application.Services.LoggingError
{
    public class ExceptionResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public List<ErrorLogEntryModel> ListError { get; set; }
    }
}
