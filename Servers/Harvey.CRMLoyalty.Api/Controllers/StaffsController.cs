using Harvey.CRMLoyalty.Application.Domain.Staffs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harvey.CRMLoyalty.Api.Controllers
{
    [Authorize(Roles = "Administrator, AdminStaff")]
    [Produces("application/json")]
    [Route("api/Staffs")]
    public class StaffsController : Controller
    {
        private IGetStaffsQuery _getStaffsQuery;
        public StaffsController(IGetStaffsQuery getStaffsQuery)
        {
            _getStaffsQuery = getStaffsQuery;
        }

        [HttpGet("gets")]
        public IActionResult GetStaffs(GetStaffsRequest request)
        {
            var result = _getStaffsQuery.GetStaffs(request);
            return Ok(result);
        }

        [HttpGet("getsByOutlet")]
        public IActionResult GetStaffsByOutlet(GetStaffsByOutletRequest request)
        {
            var result = _getStaffsQuery.GetStaffsByOutlet(request);
            return Ok(result);
        }
    }
}