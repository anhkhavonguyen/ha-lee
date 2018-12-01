using System.Threading.Tasks;
using Harvey.Activity.Application.Services;
using Harvey.Message.Activities;
using MassTransit;

namespace Harvey.Activity.Application.Consumers.Activity
{
    public class LoggingActivityConsumer : IConsumer<LoggingActivityCommand>
    {
        private readonly ILoggingActivityService _loggingActivityService;
        public LoggingActivityConsumer(ILoggingActivityService loggingActivityService)
        {
            _loggingActivityService = loggingActivityService;
        }

        public async Task Consume(ConsumeContext<LoggingActivityCommand> context)
        {
            await _loggingActivityService.ExecuteAsync(new LoggingActivityRequest
            {
                UserId = context.Message.UserId,
                ActionAreaPath = context.Message.ActionAreaPath,
                ActionType = context.Message.ActionType,
                Comment = context.Message.Comment,
                Description = context.Message.Description,
                CreatedByName = context.Message.CreatedByName,
                Value = context.Message.Value
            });
        }
    }
}
