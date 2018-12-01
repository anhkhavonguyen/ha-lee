using Autofac;
using Autofac.Extensions.DependencyInjection;
using Harvey.Ids.Application.Accounts;
using Harvey.Ids.Application.User;
using Harvey.Ids.Configs;
using Harvey.Ids.Consumers.Customer;
using Harvey.Ids.Consumers.Customers;
using Harvey.Ids.Domains;
using Harvey.Ids.Services;
using Harvey.Ids.Services.Account;
using Harvey.Ids.Services.Account.User;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Hosting;
using IdentityServer4.Validation;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using SendGrid;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harvey.Ids
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration _configuration { get; }
        private IContainer _container;
        private readonly IHostingEnvironment _hostingEnvironment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

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
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<HarveyIdsDbContext>(options =>
            {
                options.UseNpgsql(connectionString, cfg => cfg.MigrationsAssembly(migrationsAssembly));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.SignIn.RequireConfirmedPhoneNumber = true;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<HarveyIdsDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<ConfigurationRabbitMq>(_configuration.GetSection("RabbitMqConfig"));
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            var identityBuilder = services.AddIdentityServer(options =>
            {
                options.IssuerUri = _configuration["Authority"];
            });
            if (_hostingEnvironment.IsDevelopment())
            {
                identityBuilder.AddDeveloperSigningCredential();
            }
            identityBuilder
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(connectionString,
                        npgsql => npgsql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(connectionString,
                        npgsql => npgsql.MigrationsAssembly(migrationsAssembly));

                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            }).Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression();

            services.AddSwaggerGen(c =>
            {
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.SwaggerDoc("v1", new Info { Title = "User Management API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
            services.AddAuthentication();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                   .AddIdentityServerAuthentication("Bearer", options =>
                   {
                       options.Authority = _configuration["Authority"];
                       options.ApiName = "Harvey.UserManagement.Api";
                       options.RequireHttpsMetadata = false;
                   });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<ISendGridClient>(new SendGridClient(_configuration["EmailSender:ApiKey"]));

            services.AddOptions();
            services.Configure<EmailSenderOptions>(_configuration.GetSection("EmailSender"));

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<InitCustomerProfileConsumer>();
                cfg.AddConsumer<ActiveCustomerConsumer>();
                cfg.AddConsumer<ReactiveCustomerWithNewPhoneConsumer>();
            });

            MappingConfiguration.Execute();
            AccountApplicationModule.Registry(services);
            ServiceRegistryModule.Registry(services);
            UserApplicationModule.Register(services);

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
                app.UseDatabaseErrorPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });

            app.UseIdentityServer();
            app.UseStaticFiles();
            //TODO remove for RIMS need to revit for CRM intergration tests
            //app.UseMiddleware<PublicfacingUrlMiddleware>(_configuration["Authority"]);
            app.ConfigureCors();
            app.UseMiddleware<IdentityServerMiddleware>();

            app.UseResponseCompression();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
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
