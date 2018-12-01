using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetDeactivatedCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities;
using Harvey.Activity.Application.Services;
using Harvey.Activity.Application.Services.LoggingError;
using Microsoft.Extensions.DependencyInjection;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime;

namespace Harvey.Activity.Application
{
    public static class ApplicationModule
    {
        public static void Registry(IServiceCollection services)
        {
            services.AddScoped<ILoggingActivityService, LoggingActivityService>();
            services.AddScoped<ILoggingErrorService, LoggingErrorService>();
            services.AddScoped<IGetActivitiesQuery, GetActivitiesQuery>();
            services.AddScoped<IGetHistoryChangeNumberCustomerQuery, GetHistoryChangeNumberCustomerQuery>();
            services.AddScoped<IGetCustomerActivitiesQuery, GetCustomerActivitiesQuery>();
            services.AddScoped<IGetActivatedCustomerActivitiesQuery, GetActivatedCustomerActivitiesQuery>();
            services.AddScoped<IGetDeactivatedCustomerActivitiesQuery, GetDeactivatedCustomerActivitiesQuery>();
            services.AddScoped<IGetVisitorsStatistics, GetVisitorsStatistics>();
        }
    }
}
