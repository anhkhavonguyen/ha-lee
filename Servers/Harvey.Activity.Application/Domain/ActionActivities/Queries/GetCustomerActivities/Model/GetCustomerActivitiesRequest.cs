using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities.Model
{
    public class GetCustomerActivitiesRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string CustomerCode { get; set; }
    }
}
