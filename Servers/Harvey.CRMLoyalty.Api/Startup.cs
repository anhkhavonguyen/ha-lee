using Autofac;
using Autofac.Extensions.DependencyInjection;
using GreenPipes;
using Harvey.CRMLoyalty.Api.Middleware;
using Harvey.CRMLoyalty.Api.Securities;
using Harvey.CRMLoyalty.Application;
using Harvey.CRMLoyalty.Application.Configuration;
using Harvey.CRMLoyalty.Application.Consumers.Customers;
using Harvey.CRMLoyalty.Application.Consumers.MembershipTransactions;
using Harvey.CRMLoyalty.Application.Consumers.PointTransactions;
using Harvey.CRMLoyalty.Application.Domain;
using Harvey.CRMLoyalty.Application.Services;
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

namespace Harvey.CRMLoyalty.Api
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

            var containerBuilder = new ContainerBuilder();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HarveyCRMLoyaltyDbContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    optionsBuilder =>
                     optionsBuilder.MigrationsAssembly(migrationsAssembly));
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression();
            services.AddMvc();

            SecurityRegister.AddAuthentication(services, _configuration);
            SecurityRegister.AddAuthorization(services);

            services.Configure<ConfigurationRabbitMq>(_configuration.GetSection("RabbitMqConfig"));

            services.AddSwaggerGen(c =>
            {
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.SwaggerDoc("v1", new Info { Title = "CRM Loyaty API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

            MappingConfiguration.Execute();

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<UpdateMemberProfileConsumer>();
                cfg.AddConsumer<UpdateFullCustomerInfomationConsumer>();
                cfg.AddConsumer<UpdateCustomerProfileAfterInitConsumer>();
                cfg.AddConsumer<UpdateGenderValueConsumer>();
                cfg.AddConsumer<ChangePhoneNumberConsumer>();
                cfg.AddConsumer<ExpiryPointConsumers>();
                cfg.AddConsumer<ExpiryMembershipNotificationConsumer>();
                cfg.AddConsumer<ExpiryRewardPointNotificationConsumer>();
            });

            ServiceApplicationModule.Registry(services);
            DomainApplicationModule.Registry(services);

            containerBuilder.Populate(services);
            containerBuilder.RegisterModule<BusModule>();
            
            _container = containerBuilder.Build();
            return new AutofacServiceProvider(_container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
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
            app.UseResponseCompression();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Loyaty API V1");
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.Full);
            });

            try
            {
                var bus = app.ApplicationServices.GetService<IBusControl>();
                var busHandle = TaskUtil.Await(() => bus.StartAsync());
                lifetime.ApplicationStopping.Register(() => busHandle.Stop());
            }
            catch (Exception) { }
        }
    }
}
