using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities.Model
{
    public class GetHistoryChangeNumberCustomerRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string CustomerCode { get; set; }
        public string ActionType { get; set; }
    }
}
