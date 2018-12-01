using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Services.LoggingError
{
    public class ErrorRequest
    {
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCaption { get; set; }
        public string Source { get; set; }
    }

    public class ErrorLogRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
