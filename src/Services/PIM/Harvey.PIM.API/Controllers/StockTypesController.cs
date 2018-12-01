using Harvey.Persitance.EF;
using Harvey.PIM.Application.Infrastructure;
using Harvey.PIM.Application.Infrastructure.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harvey.PIM.API.Controllers
{
    [Authorize]
    [Route("api/{stocktypes}")]
    public class StockTypesController : ControllerBase
    {
        private readonly IEfRepository<TransactionDbContext, StockType> _efRepository;
        public StockTypesController(IEfRepository<TransactionDbContext, StockType> efRepository)
        {
            _efRepository = efRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockType>>> GetAll()
        {
            return Ok(await _efRepository.GetAsync());
        }
    }
}
