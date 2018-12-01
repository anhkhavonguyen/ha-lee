using Harvey.Activity.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities.Model
{
    public class GetCustomerActivitiesResponse
    {
        public List<ActionActivityModel> ActionModels { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
    }
}
