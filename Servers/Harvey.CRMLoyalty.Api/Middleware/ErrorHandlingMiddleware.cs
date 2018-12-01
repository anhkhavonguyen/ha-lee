using Harvey.CRMLoyalty.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate nextRequest;
        private readonly ILogger _logger;
        private readonly ILoggingErrorService _loggingError;
        public ErrorHandlingMiddleware(RequestDelegate nextRequest, ILogger<ErrorHandlingMiddleware> logger, ILoggingErrorService loggingError)
        {
            this.nextRequest = nextRequest;
            _logger = logger;
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
                _logger.LogError(ex.GetBaseException().ToString());
            }
        }
    }
}
