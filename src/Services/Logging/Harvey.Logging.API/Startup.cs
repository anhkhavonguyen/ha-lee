using Harvey.Logging.API.Extensions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Harvey.Logging.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            services.AddHttpContextAccessor();

            services
                .AddEventBus(Configuration);

            services.AddAuthorization();

            services
               .AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = Configuration["Authority"];
                   options.RequireHttpsMetadata = false;
                   options.SupportedTokens = SupportedTokens.Reference;
                   options.ApiName = "harvey.rims.api";
                   options.ApiSecret = "secret";
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });

            app.UseAuthentication();

            app.UseMvc();

            app.ConfigureEventBus();
        }
    }
}
