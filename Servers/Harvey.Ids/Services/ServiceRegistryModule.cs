using Harvey.Ids.Services.Activity;
using Harvey.Ids.Services.ClientContextService;
using Harvey.Ids.Services.GenerateShortLinkFromTinyUrl;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Ids.Services
{
    public static class ServiceRegistryModule
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<IClientContextService, ClientContextService.ClientContextService>();
            services.AddScoped<IGenerateShortLinkFromTinyUrlService, GenerateShortLinkFromTinyUrlService>();
            services.AddScoped<ILoggingActivityService, LoggingActivityService>();
        }
    }
}
