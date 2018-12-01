using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities.Model;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities
{
    public interface IGetActivatedCustomerActivitiesQuery
    {
        GetActivatedCustomerActivitiesResponse Execute(GetActivatedCustomerActivitiesRequest request);
    }
}
