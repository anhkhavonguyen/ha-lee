using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Harvey.Exception.Handlers
{
    public class SqlExceptionHandler : IExceptionHandler
    {
        private const string GENERIC_ERROR_MESSAGE = "Something went wrong! Please contact administrators";
        public ProblemDetails Handle(System.Exception ex, ref bool hasHandle)
        {
            var exception = ex;
            ProblemDetails result = new ProblemDetails();
            string status = null;
            if (exception is SqlException)
            {
                var sqlException = exception as SqlException;
                // Remove any 'sql' string from the message (penetration testing report)
                var message = Regex.Replace(exception.Message, "sql", "", RegexOptions.IgnoreCase);
                // Handling application exceptions
                var httpStatusCode = HttpStatusCode.InternalServerError;
                if (sqlException.Number >= 50000)
                {
                    switch (sqlException.Number)
                    {
                        case 54002:
                            httpStatusCode = HttpStatusCode.Forbidden;
                            status = "Forbidden";
                            break;
                        case 54001:
                            httpStatusCode = HttpStatusCode.NotFound;
                            status = "Not Found";
                            break;
                        case 54000:
                        default:
                            httpStatusCode = HttpStatusCode.BadRequest;
                            status = "Bad Request";
                            break;
                    }
                    result = new ProblemDetails()
                    {
                        Title = status,
                        Status = (int)httpStatusCode,
                        Detail = ex.Message ?? GENERIC_ERROR_MESSAGE,
                    };
                }
                else
                {
                    result = new ProblemDetails()
                    {
                        Title = "Internal Server Error",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = ex.Message ?? GENERIC_ERROR_MESSAGE,
                    };
                }
                hasHandle = true;

            }
            return result;
        }
    }
}
