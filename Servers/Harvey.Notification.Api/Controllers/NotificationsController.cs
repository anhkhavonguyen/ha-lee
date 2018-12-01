using Harvey.Notification.Application.Domains.Notifications.Queries;
using Harvey.Notification.Application.Services.LoggingError;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.Notification.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Notifications")]
    public class NotificationsController : Controller
    {
        private readonly IGetNotificationsQuery _getNotificationsQuery;
        private readonly ILoggingErrorService _loggingErrorService;

        public NotificationsController(IGetNotificationsQuery getNotificationsQuery, ILoggingErrorService loggingErrorService)
        {
            _getNotificationsQuery = getNotificationsQuery;
            _loggingErrorService = loggingErrorService;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetNotifications(GetNotificationsRequest request)
        {
            var result = _getNotificationsQuery.Execute(request);
            return Ok(result);
        }
    }
}
