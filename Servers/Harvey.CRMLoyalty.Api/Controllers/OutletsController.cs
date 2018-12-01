using Harvey.CRMLoyalty.Api.Utils;
using Harvey.CRMLoyalty.Application.Domain.Outlets.Commands.UpdateOutletCommandHandler;
using Harvey.CRMLoyalty.Application.Domain.Outlets.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Harvey.CRMLoyalty.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Outlets")]
    public class OutletsController : Controller
    {
        private IGetOutletsQuery _getOutletsQuery;
        private IUpdateOutletCommandHandler _UpdateOutletCommandHandler;
        public OutletsController(
            IGetOutletsQuery getOutletsQuery,
            IUpdateOutletCommandHandler UpdateOutletCommandHandler)
        {
            _getOutletsQuery = getOutletsQuery;
            _UpdateOutletCommandHandler = UpdateOutletCommandHandler;
        }

        [HttpGet("gets")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetOutlets(GetOutletsRequest request)
        {
            var result = _getOutletsQuery.GetOutlets(request);
            return Ok(result);
        }

        [HttpGet("getsbystaff")]
        [Authorize(Roles = "Staff,AdminStaff")]
        public IActionResult GetOutletsByStaff(GetOutletsRequest request)
        {
            var result = _getOutletsQuery.GetOutletsByStaff(request);
            return Ok(result);
        }


        [HttpGet("getOutletsWithStoreAccount")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public IActionResult GetOutletsWithStoreAccount(GetOutletsRequest request)
        {
            var result = _getOutletsQuery.GetOutletsWithStoreAccount(request);
            return Ok(result);
        }

        [HttpGet("getsWithoutPaging")]
        [AllowAnonymous]
        public IActionResult GetOutlets()
        {
            var result = _getOutletsQuery.GetOutlets();
            return Ok(result);
        }

        [HttpPost("updateOutlet")]
        [Authorize(Roles = "Administrator,AdminStaff")]
        public async Task<IActionResult> UpdateOutlet([FromBody] UpdateOutletCommand request)
        {
            request.UserId = User.GetUserId();
            var result = await _UpdateOutletCommandHandler.ExecuteAsync(request);
            return Ok(result);
        }
    }
}