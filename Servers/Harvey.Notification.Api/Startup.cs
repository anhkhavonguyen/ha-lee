using Autofac;
using Autofac.Extensions.DependencyInjection;
using GreenPipes;
using Harvey.Notification.Api.Middleware;
using Harvey.Notification.Api.Securities;
using Harvey.Notification.Application;
using Harvey.Notification.Application.Consumers.Accounts;
using Harvey.Notification.Application.Consumers.Notifications;
using Harvey.Notification.Application.Domains.Accounts;
using Harvey.Notification.Application.Domains.Notifications;
using Harvey.Notification.Application.Services;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harvey.Notification.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }
        private IContainer _container { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
            };
            Log.Logger = new LoggerConfiguration()
                           .Enrich.WithMachineName()
                           .MinimumLevel.Information()
                           .ReadFrom.Configuration(_configuration)
                           .WriteTo.PostgreSQL(_configuration["Serilog:WriteTo:Args:connectionString"],
                                               _configuration["Serilog:WriteTo:Args:tableName"],
                                               columnOptions: columnWriters,
                                               needAutoCreateTable: true)
                            .CreateLogger();

            SecurityRegister.AddAuthentication(services, _configuration);
            SecurityRegister.AddAuthorization(services);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.SwaggerDoc("v1", new Info { Title = "CRM Notification API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

            services.AddMvc();
            services.AddMassTransit(c =>
            {
                c.AddConsumer<SendAllPendingSmsConsumer>();
                c.AddConsumer<SendForgotPasswordEmailConsumer>();
                c.AddConsumer<SendPINToNumberPhoneConsumer>();
                c.AddConsumer<InitMemberAccountConsumer>();
                c.AddConsumer<ReSendSignUpLinkConsumer>();
                c.AddConsumer<SendForgotPasswordSMSConsumer>();
                c.AddConsumer<SendSmsChangePhoneNumberConsumer>();
                c.AddConsumer<SendExpiryMembershipNotificationConsumer>();
                c.AddConsumer<SendExpiryRewardPointNotificationConsumer>();
            });
            services.AddDbContext<HarveyNotificationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, cfg =>
                {
                    cfg.MigrationsAssembly(migrationsAssembly);
                });
            });
            MappingConfiguration.Execute();
            AccountApplicationModule.Registry(services);
            NotificationApplicationModule.Registry(services);
            ServiceModuleRegister.Registry(services);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);
            containerBuilder.RegisterModule<BusModule>();
            _container = containerBuilder.Build();
            return new AutofacServiceProvider(_container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IServiceProvider serviceProvider,
            IHostingEnvironment env,
            IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
            });

            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API V1");
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.Full);
            });

            try
            {
                var bus = app.ApplicationServices.GetService<IBusControl>();
                var busHandle = TaskUtil.Await(() => bus.StartAsync());
                lifetime.ApplicationStopping.Register(() => busHandle.Stop());
                lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            }
            catch (Exception) { }
        }
    }
}
