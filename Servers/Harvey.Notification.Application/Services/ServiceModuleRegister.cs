using Harvey.Notification.Application.Services.EmailService;
using Harvey.Notification.Application.Services.LoggingError;
using Harvey.Notification.Application.Services.SMSService;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Notification.Application.Services
{
    public class ServiceModuleRegister
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService.EmailService>();
            services.AddScoped<ISMSService, SMSService.SMSService>();
            services.AddScoped<ILoggingErrorService, LoggingErrorService>();
        }
    }
}
