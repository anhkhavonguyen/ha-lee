using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities
{
    public interface IGetCustomerActivitiesQuery
    {
        GetCustomerActivitiesResponse Execute(GetCustomerActivitiesRequest request);
    }
}
