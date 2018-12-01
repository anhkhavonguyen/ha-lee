using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities.Model
{
    public class GetActivitiesRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? FromDateFilter { get; set; }
        public DateTime? ToDateFilter { get; set; }
        public string SearchText { get; set; }
    }
}
