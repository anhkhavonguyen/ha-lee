using Harvey.Activity.Application.Services.LoggingError;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Activity.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate nextRequest;
        private readonly ILoggingErrorService _loggingError;
        public ErrorHandlingMiddleware(RequestDelegate nextRequest, ILoggingErrorService loggingError)
        {
            this.nextRequest = nextRequest;
            _loggingError = loggingError;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await nextRequest(context);
            }
            catch (Exception ex)
            {
                var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == "sub");
                var userId = idClaim != null ? idClaim.Value : "Unknown";
                var isBackEndSource = true;
                _loggingError.LogError(userId, ex, isBackEndSource);
            }
        }
    }
}
