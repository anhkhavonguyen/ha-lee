using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Application.Services.Activity
{
    public interface ILoggingActivityService
    {
        Task ExecuteAsync(LoggingActivityRequest request, string RabbitMqUrl);
    }
}
