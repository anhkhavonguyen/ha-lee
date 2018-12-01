using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Notification.Api.Securities
{
    public static class SecurityRegister
    {
        public static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication("Bearer", options =>
                    {
                        options.Authority = configuration["Authority"];
                        options.ApiName = "Harvey.Notification.Api";
                        options.RequireHttpsMetadata = false;
                    });
        }

        public static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization();
        }
    }
}
