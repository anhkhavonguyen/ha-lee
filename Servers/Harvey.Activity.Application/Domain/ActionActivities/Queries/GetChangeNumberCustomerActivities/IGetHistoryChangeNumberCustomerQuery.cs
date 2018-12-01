using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities
{
    public interface IGetHistoryChangeNumberCustomerQuery
    {
        GetHistoryChangeNumberCustomerRespone Execute (GetHistoryChangeNumberCustomerRequest request);
    }
}
