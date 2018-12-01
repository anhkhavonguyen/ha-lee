using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harvey.Ids.Services.Activity
{
    public interface ILoggingActivityService
    {
        Task ExecuteAsync(LoggingActivityRequest request, string RabbitMqUrl);
    }
}
