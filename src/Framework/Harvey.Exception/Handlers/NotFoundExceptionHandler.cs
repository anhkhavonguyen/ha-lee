using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Harvey.Exception.Handlers
{
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        private const string GENERIC_ERROR_MESSAGE = "resource is not found.";
        public ProblemDetails Handle(System.Exception ex, ref bool hasHandle)
        {
            var result = new ProblemDetails();
            if (ex is NotFoundException)
            {
                result = new ProblemDetails()
                {
                    Title = "Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = ex.Message ?? GENERIC_ERROR_MESSAGE,
                };
                hasHandle = true;
            }
            return result;
        }
    }
}
