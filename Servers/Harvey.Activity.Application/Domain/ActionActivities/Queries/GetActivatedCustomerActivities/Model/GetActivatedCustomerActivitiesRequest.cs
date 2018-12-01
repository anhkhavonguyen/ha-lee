using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities.Model
{
    public class GetActivatedCustomerActivitiesRequest
    {
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
