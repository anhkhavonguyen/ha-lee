using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Harvey.Logging.API.Controllers
{
    [Route("api/logs")]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        public LogsController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] LogEntry entry)
        {
            if (!Enum.IsDefined(typeof(LogLevel), (int)entry.LogLevel))
            {
                throw new ArgumentException($"{entry.LogLevel.ToString()} is not supported.");
            }
            if (!Enum.IsDefined(typeof(Application), (int)entry.Application))
            {
                throw new ArgumentException("Application is required.");
            }
            if (string.IsNullOrEmpty(entry.Message))
            {
                throw new ArgumentException("Message is required.");
            }
            await _eventBus.PublishAsync(new LoggingEvent()
            {
                LogLevel = entry.LogLevel,
                Application = entry.Application,
                Message = entry.Message
            });
            return Ok();
        }
    }
}
