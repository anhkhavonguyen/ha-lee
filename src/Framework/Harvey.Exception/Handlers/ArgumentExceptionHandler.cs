using System;
using System.Net;

namespace Harvey.Exception.Handlers
{
    public sealed class ArgumentExceptionHandler : IExceptionHandler
    {
        public ProblemDetails Handle(System.Exception ex, ref bool hasHandle)
        {
            if (ex is ArgumentException)
            {
                hasHandle = true;
                return new ProblemDetails()
                {
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = ex.Message
                };
            }
            else
            {
                return null;
            }
        }
    }
}
