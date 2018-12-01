using Harvey.Notification.Application.Services.LoggingError;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.Notification.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/LoggingError")]
    public class LoggingErrorController : Controller
    {
        private ILoggingErrorService _loggingErrorService;
        public LoggingErrorController(ILoggingErrorService loggingErrorService)
        {
            _loggingErrorService = loggingErrorService;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetErrorLog(ErrorLogRequest request)
        {
            var result = _loggingErrorService.GetErrorLog(request);
            return Ok(result);
        }
    }
}