using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetDeactivatedCustomerActivities.Model;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetDeactivatedCustomerActivities
{
    public interface IGetDeactivatedCustomerActivitiesQuery
    {
        GetDeactivatedCustomerActivitiesResponse Execute(GetDeactivatedCustomerActivitiesRequest request);
    }
}
