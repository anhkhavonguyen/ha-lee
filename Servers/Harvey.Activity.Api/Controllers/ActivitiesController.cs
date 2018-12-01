using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivities.Model;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetChangeNumberCustomerActivities.Model;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetCustomerActivities.Model;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetDeactivatedCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetDeactivatedCustomerActivities.Model;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetActivatedCustomerActivities.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime.Model;
using Harvey.Activity.Application.Domain.ActionActivities.Queries.GetVisitorsInPeriodTime;

namespace Harvey.Activity.Api.Controllers
{
    [Route("api/Activity")]
    public class ActivitiesController : Controller
    {
        private readonly IGetActivitiesQuery _getActivitiesQuery;
        private readonly IGetHistoryChangeNumberCustomerQuery _getChangeNumberCustomerActivitiesQuery;
        private readonly IGetCustomerActivitiesQuery _getCustomerActivitiesQuery;
        private readonly IGetActivatedCustomerActivitiesQuery _getActivatedCustomerActivitiesQuery;
        private readonly IGetDeactivatedCustomerActivitiesQuery _getDeactivatedCustomerActivitiesQuery;
        private readonly IGetVisitorsStatistics _getVisitorsStatistics;
        public ActivitiesController(IGetActivitiesQuery getActivitiesQuery,
            IGetHistoryChangeNumberCustomerQuery getChangeNumberCustomerActivitiesQuery,
            IGetCustomerActivitiesQuery getCustomerActivitiesQuery,
            IGetActivatedCustomerActivitiesQuery getActivatedCustomerActivitiesQuery,
            IGetDeactivatedCustomerActivitiesQuery getDeactivatedCustomerActivitiesQuery,
            IGetVisitorsStatistics getVisitorsStatistics)
        {
            _getActivitiesQuery = getActivitiesQuery;
            _getChangeNumberCustomerActivitiesQuery = getChangeNumberCustomerActivitiesQuery;
            _getCustomerActivitiesQuery = getCustomerActivitiesQuery;
            _getActivatedCustomerActivitiesQuery = getActivatedCustomerActivitiesQuery;
            _getDeactivatedCustomerActivitiesQuery = getDeactivatedCustomerActivitiesQuery;
            _getVisitorsStatistics = getVisitorsStatistics;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetNotifications(GetActivitiesRequest request)
        {
            var result = _getActivitiesQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getHistoryChangeNumberCustomer")]
        [Authorize(Roles = "Administrator,AdminStaff,Member")]
        public IActionResult GetHistoryChangeNumberCustomer(GetHistoryChangeNumberCustomerRequest request)
        {
            var result = _getChangeNumberCustomerActivitiesQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getHistoryCustomerActivities")]
        [Authorize(Roles = "Administrator,AdminStaff,Member")]
        public IActionResult GetHistoryCustomerActivities(GetCustomerActivitiesRequest request)
        {
            var result = _getCustomerActivitiesQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("GetHistoryActivatedCustomerActivities")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetHistoryActivatedCustomerActivities(GetActivatedCustomerActivitiesRequest request)
        {
            var result = _getActivatedCustomerActivitiesQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getHistoryDeactivatedCustomerActivities")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult getHistoryDeactivatedCustomerActivities(GetDeactivatedCustomerActivitiesRequest request)
        {
            var result = _getDeactivatedCustomerActivitiesQuery.Execute(request);
            return Ok(result);
        }

        [HttpGet("getVisitorsStatistics")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult getVisitorsStatistics(GetVisitorsStatisticsRequest request)
        {
            var result = _getVisitorsStatistics.Execute(request);
            return Ok(result);
        }
    }
}