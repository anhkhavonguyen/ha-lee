using Harvey.Persitance.EF;
using Harvey.PIM.API.Filters;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Harvey.PIM.Application.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.API.Controllers
{
    [Route("api/prices")]
    [Authorize]
    [ServiceFilter(typeof(ActivityTracking))]
    public class PricesController : ControllerBase
    {
        private readonly IEfRepository<PimDbContext, Price, PriceModel> _efRepository;
        public PricesController(IEfRepository<PimDbContext, Price, PriceModel> efRepository)
        {
            _efRepository = efRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceModel>>> Get()
        {
            return Ok(await _efRepository.GetAsync());
        }
    }
}
