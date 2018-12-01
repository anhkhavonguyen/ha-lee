using Harvey.Exception.Extensions;
using Harvey.Logging;
using Harvey.Persitance.EF;
using Harvey.PIM.API.Extensions;
using Harvey.PIM.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Harvey.PIM.API.Filters
{
    public class ActivityTracking : IAsyncActionFilter
    {
        private readonly ILogger<ActivityTracking> _logger;
        private readonly IEfRepository<ActivityLogDbContext, ActivityLog> _efRepository;
        public ActivityTracking(
            ILogger<ActivityTracking> logger,
            IEfRepository<ActivityLogDbContext, ActivityLog> efRepository)
        {
            _logger = logger;
            _efRepository = efRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var controller = context.Controller as ControllerBase;
            if (controller == null)
            {
                return;
            }
            var hasError = resultContext.Exception != null;
            var message = $"[{resultContext.HttpContext.Connection.RemoteIpAddress.ToString()}] [{controller.RouteData.Values["controller"]}] [{controller.RouteData.Values["action"]}]";
            var activity = new ActivityLog()
            {
                Application = Logging.Application.PIM,
                Date = DateTime.Now,
                IPAddress = resultContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                LogLevel = hasError ? LogLevel.Error : LogLevel.Information,
                Context = message,
                UserId = resultContext.HttpContext.User.GetUserId(),
                UserName = resultContext.HttpContext.User.GetUserId().ToString()//should get userName
            };
            if (hasError)
            {
                activity.Message = resultContext.Exception.GetTraceLog();
            }
            await _efRepository.AddAsync(activity);
            await _efRepository.SaveChangesAsync();
            var logMessage = $"[Activity Tracking] [{activity.UserId}] [{activity.UserName}] [{activity.Context}]";
            if (hasError)
            {
                logMessage += $" [{activity.Message}]";
                _logger.LogError(logMessage);
                throw resultContext.Exception;
            }
            else
            {
                _logger.LogInformation(logMessage);
            }
        }
    }
}
