using Harvey.CRMLoyalty.Application.Services.Activity;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.CRMLoyalty.Application.Services
{
    public static class ServiceApplicationModule
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<ILoggingErrorService, LoggingErrorService>();
            services.AddScoped<ILoggingActivityService, LoggingActivityService>();
        }
    }
}

