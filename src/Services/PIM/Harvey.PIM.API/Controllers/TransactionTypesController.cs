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
    [Route("api/{transactiontypes}")]
    public class TransactionTypesController : ControllerBase
    {
        private readonly IEfRepository<TransactionDbContext, TransactionType> _efRepository;
        public TransactionTypesController(IEfRepository<TransactionDbContext, TransactionType> efRepository)
        {
            _efRepository = efRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetAll()
        {
            return Ok(_efRepository.GetAsync());
        }
    }
}
