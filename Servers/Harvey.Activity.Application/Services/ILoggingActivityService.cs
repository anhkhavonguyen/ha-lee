using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Harvey.Activity.Application.Services
{
    public interface ILoggingActivityService
    {
        Task ExecuteAsync(LoggingActivityRequest command);
    }
}
