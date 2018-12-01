using Harvey.PIM.API.Extensions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Harvey.PIM.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHttpContextAccessor();

            services
                .AddServiceDbContext(Configuration)
                .AddEventBus(Configuration)
                .AddCQRS(Configuration)
                .AddRepositories(Configuration)
                .AddFieldFramework(Configuration)
                .AddAppSetting(Configuration)
                .AddLogger(Configuration)
                .AddMapper(Configuration)
                .AddExceptionHandlers(Configuration)
                .AddProvisionTask(Configuration)
                .AddMarketingAutomation(Configuration)
                .AddJobManager(Configuration)
                .AddTrackingActivity(Configuration)
                .AddSearchService(Configuration)
                .AddAssignmentServices(Configuration);

            services.AddAuthorization();

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = Configuration["Authority"];
                    options.SupportedTokens = SupportedTokens.Reference;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "harvey.rims.api";
                    options.ApiSecret = "secret";
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info()
                {
                    Title = "PIM API",
                    Version = "v1"
                });

                options.DescribeAllEnumsAsStrings();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter Token with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });

            services.AddCors();
            ;
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

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PIM API V1");
            });

            app.UseStaticFiles();

            app.ConfigureExceptionHandler(env);

            app.UseMvc();

            app.ConfigureJobManager(env);

            app.ConfigureMarketingAutomation(env);

            app.ConfigureEventBus();
        }
    }
}
