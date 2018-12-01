using Hangfire.Dashboard;

namespace Harvey.Notification.Api.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var user = httpContext.User;
            if (user.Identity.IsAuthenticated && user.IsInRole("Administrator"))
            {
                return true;
            }
            return false;
        }
    }
}
