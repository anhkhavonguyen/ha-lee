using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities
{
    public interface IGetActivitiesQuery
    {
        GetActivitiesResponse Execute(GetActivitiesRequest request);
    }
}
