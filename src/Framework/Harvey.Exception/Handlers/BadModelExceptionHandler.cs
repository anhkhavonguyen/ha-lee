using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Harvey.Exception.Handlers
{
    public class BadModelExceptionHandler : IExceptionHandler
    {
        private const string GENERIC_ERROR_MESSAGE = "The argument cannot be null";
        public ProblemDetails Handle(System.Exception ex, ref bool hasHandle)
        {
            ProblemDetails result = new ProblemDetails();
            if (ex is BadModelException)
            {
                result = new ProblemDetails()
                {
                    Title = "Bad Request",
                    Detail = ex.Message,
                    Status = (int)HttpStatusCode.BadRequest
                };
                hasHandle = true;
            }
            if (ex is NullModelException)
            {
                result = new ProblemDetails()
                {
                    Title = "Bad Request",
                    Detail = ex.Message ?? GENERIC_ERROR_MESSAGE,
                    Status = (int)HttpStatusCode.BadRequest
                };
                hasHandle = true;
            }
            return result;
        }
    }
}
