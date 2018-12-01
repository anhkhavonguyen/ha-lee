using Harvey.CRMLoyalty.Application.Services;
using Harvey.CRMLoyalty.Application.Services.LoggingError;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Harvey.CRMLoyalty.Api.Controllers
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

        [HttpPost("logError")]
        [AllowAnonymous]
        public IActionResult LogError([FromBody] ErrorRequest request)
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            request.UserId = idClaim != null ? idClaim.Value : "Unknown";
            return Ok(_loggingErrorService.LogError(request));
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetErrorLog(ErrorLogRequest request)
        {
            var result = _loggingErrorService.GetErrorLog(request);
            return Ok(result);
        }
    }
}