using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Harvey.Domain
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandExecutor> _logger;
        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = (ILogger<CommandExecutor>)_serviceProvider.GetService(typeof(ILogger<CommandExecutor>));
        }
        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var htype = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(htype);
            _logger.LogTrace($"[Command Executor] [{handler.GetType().Name}] {JsonConvert.SerializeObject(command)}");
            return handler.Handle((dynamic)command);
        }
    }
}
