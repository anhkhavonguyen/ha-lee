using Harvey.CRMLoyalty.Application.Entities;
using Harvey.CRMLoyalty.Application.Models;
using Harvey.CRMLoyalty.Application.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.CRMLoyalty.Application.Services.LoggingError
{
    public class ExceptionResponse : BaseResponse
    {
        public List<ErrorLogEntryModel> ListError { get; set; }
    }


}
