using Harvey.Activity.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities.Model
{
    public class GetActivitiesResponse
    {
        public List<ActionActivityModel> ActionActivityModels { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
    }
}
