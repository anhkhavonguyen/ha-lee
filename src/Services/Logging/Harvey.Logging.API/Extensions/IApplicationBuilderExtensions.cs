using Harvey.EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Harvey.Logging.API.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.AuthorIdResolver = () =>
            {
                var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
                var idClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub");
                if (idClaim == null)
                {
                    return default(Guid);
                }
                return Guid.Parse(idClaim.Value);
            };
            eventBus.Commit();
        }
    }
}
