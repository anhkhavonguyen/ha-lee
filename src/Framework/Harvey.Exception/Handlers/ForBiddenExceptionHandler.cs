using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Harvey.Exception.Handlers
{
    public class ForBiddenExceptionHandler : IExceptionHandler
    {
        private const string GENERIC_ERROR_MESSAGE = "request is not allowed.";
        public ProblemDetails Handle(System.Exception ex, ref bool hasHandle)
        {
            ProblemDetails result = new ProblemDetails();
            if (ex == null)
            {
                return result;
            }
            if (ex is ForBiddenException)
            {
                result = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.Forbidden,
                    Detail = ex.Message ?? GENERIC_ERROR_MESSAGE,
                    Title = "Forbidden"
                };
                hasHandle = true;
            }
            return result;
        }
    }
}
