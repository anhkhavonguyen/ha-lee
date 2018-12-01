using Harvey.EventBus.Abstractions;
using Harvey.EventBus.Events;
using Harvey.EventBus.Publishers;
using Harvey.Exception;
using Harvey.Exception.Handlers;
using Harvey.PIM.Application.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Newtonsoft.Json;
using Harvey.Exception.Extensions;
using System.Net;
using Newtonsoft.Json.Serialization;
using Harvey.PIM.Application.Events.Products;
using Harvey.PIM.MarketingAutomation;
using Harvey.PIM.Application.Channels;
using Hangfire;
using Harvey.Job.Hangfire;
using Harvey.EventBus.Events.Products;
using Harvey.PIM.Application.EventHandlers.Products;
using Microsoft.Extensions.Logging;
using Harvey.PIM.Application.EventHandlers.FieldValues;
using Harvey.EventBus.Events.FileldValues;
using Harvey.EventBus.Events.Variants;
using Harvey.PIM.Application.EventHandlers.Variants;
using Harvey.EventBus.Events.Prices;

namespace Harvey.PIM.API.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.AuthorIdResolver = () =>
            {
                var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
                if (httpContextAccessor.HttpContext == null)
                {
                    return default(Guid);
                }
                var idClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub");
                if (idClaim == null)
                {
                    return default(Guid);
                }
                return Guid.Parse(idClaim.Value);
            };
            eventBus.AddSubcription<LoggingPublisher, LoggingEvent, LoggingEventHandler>();

            eventBus.AddSubcription<ProductPublisher, ProductCreatedEvent, ProductCreatedEventHandler>();
            eventBus.AddSubcription<ProductPublisher, ProductUpdatedEvent, ProductUpdatedEventHandler>();
            eventBus.AddSubcription<ProductPublisher, FieldValueCreatedEvent, ProductFieldValueCreatedEventHandler>();
            eventBus.AddSubcription<ProductPublisher, FieldValueUpdatedEvent, ProductFieldValueUpdatedEventHandler>();
            eventBus.AddSubcription<ProductPublisher, VariantCreatedEvent, VariantCreatedEventHandler>();
            eventBus.AddSubcription<ProductPublisher, PriceCreatedEvent, PriceCreatedEventHandler>();
            eventBus.Commit();
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(errApp =>
            {
                errApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;
                    var hasHandle = false;
                    ProblemDetails result = null;
                    DebugProblemDetails debugResult = null;
                    result = context.RequestServices.GetRequiredService<ForBiddenExceptionHandler>().Handle(exception, ref hasHandle);

                    if (!hasHandle)
                    {
                        result = context.RequestServices.GetRequiredService<NotFoundExceptionHandler>().Handle(exception, ref hasHandle);
                    }

                    if (!hasHandle)
                    {
                        result = context.RequestServices.GetRequiredService<BadModelExceptionHandler>().Handle(exception, ref hasHandle);
                    }

                    if (!hasHandle)
                    {
                        result = context.RequestServices.GetRequiredService<ArgumentExceptionHandler>().Handle(exception, ref hasHandle);
                    }

                    if (!hasHandle)
                    {
                        result = context.RequestServices.GetRequiredService<EfUniqueConstraintExceptionHandler>().Handle(exception, ref hasHandle);
                    }

                    if (!hasHandle)
                    {
                        result = context.RequestServices.GetRequiredService<SqlExceptionHandler>().Handle(exception, ref hasHandle);
                    }

                    if (!hasHandle)
                    {
                        result = new ProblemDetails()
                        {
                            Title = "Internal Server Error",
                            Status = (int)HttpStatusCode.InternalServerError,
                            Detail = exception.Message,
                        };

                    }
                    //TODO need to revisit
                    // for now, hack allow all cors when exception occur 
                    context.Response.ContentType = "application/json";
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Accept-Encoding, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

                    var logger = context.RequestServices.GetService<ILogger<DebugProblemDetails>>();

                    if (env.IsDevelopment())
                    {
                        debugResult = new DebugProblemDetails(result)
                        {
                            DebugInfo = exception.GetTraceLog()
                        };
                        context.Response.StatusCode = debugResult.Status;
                        var message = JsonConvert.SerializeObject(debugResult, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                        logger.LogError($" [{Logging.Application.PIM}] {message}");
                        await context.Response.WriteAsync(message);
                    }
                    else
                    {
                        var message = JsonConvert.SerializeObject(result, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                        logger.LogError($" [{Logging.Application.PIM}] {message}");
                        await context.Response.WriteAsync(message);
                    }
                });
            });
        }

        public static void ConfigureMarketingAutomation(this IApplicationBuilder app, IHostingEnvironment env)
        {
            var applicationBuilder = app.ApplicationServices.GetRequiredService<ApplicationBuilder>();
            var channelConnectorInstaller = app.ApplicationServices.GetRequiredService<ChannelConnectorInstaller>();
            channelConnectorInstaller.Install(applicationBuilder);
            var marketingAutomationService = app.ApplicationServices.GetRequiredService<MarketingAutomationService>();
            marketingAutomationService.RegisterProductCreated();
            marketingAutomationService.RegisterProductUpdated();
            marketingAutomationService.RegisterCategoryCreated();
            marketingAutomationService.RegisterCategoryUpdated();
            marketingAutomationService.RegisterVariantCreated();
            marketingAutomationService.RegisterFieldValueCreated();
            marketingAutomationService.RegisterFieldValueUpdated();
            marketingAutomationService.RegisterPriceCreated();
        }

        public static void ConfigureJobManager(this IApplicationBuilder app, IHostingEnvironment env)
        {
            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.ApplicationServices));
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
